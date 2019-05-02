using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Plugin.GalenPCL;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace MoniHealth.Models
{
    public class GalenCloudComm
    {
        //private const string DeviceIdAirbox = "device-airbox";
        //private const string DevicePropertySetIdAirboxFile = "deviceset-airboxfiles";
       // private const string DevicePropertySetIdDeviceMapping = "deviceset-devicemapping";
        private const string GalenApiUrl = "https://testapi.galencloud.com";
        private const string TenantDomain = "test-ca.galencloud.com";
        private const string internetCheckUrl = "/actuator/health";
        private const string DeviceIdBPM = "device-bpm";
        private const string DevicePropertySetIdBPMFile = "deviceset-bpmfiles";
        private const string DeviceId = "deviceId";
        private const string DevicePropertySetId = "devicePropertySetId";
        private const string PropertyMappingSerialNumber = "MappingSerialNumber";
        private const string PropertyMappingUserId = "MappingUserId";
        private const string PropertySerialNumber = "SerialNumber";
        private const string PropertyFileRecordTimeStamp = "FileRecordTimeStamp";
        private const string PropertyHrfFile = "Hrf";

        DeviceData dataToSend = new DeviceData();
        private IGalenPCL Sdk;
        private string currentUserId;
        private static GalenCloudComm comm = null;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        //Initiate cloud SDK
        public static async Task<GalenCloudComm> GetCloudCommunication()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                if (comm == null)
                {
                    comm = new GalenCloudComm();
                    int init = await comm.Sdk.Init(GalenApiUrl, TenantDomain);
                    int retry = 0;
                    while (init != 0 && retry < 3)
                    {
                        init = await comm.Sdk.Init(GalenApiUrl, TenantDomain);
                        retry++;
                    }
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
            return comm;
        }

        public GalenCloudComm()
        {
            Sdk = CrossGalenPCL.Current;
        }

        ~GalenCloudComm()
        {
            Sdk.Deinit();
            Sdk = null;
        }

        private void checkAccess(bool requireLoggedIn)
        {
            if (requireLoggedIn && currentUserId == null)
            {
                throw new Exception("no-user-logged-in");
            }
            if (!hasInternet())
            {
                throw new Exception("not-connected");
            }
        }

        private Exception getCorrectException(Exception exp)
        {
            if (exp is FormatException || exp is OverflowException)
            {
                return exp;
            }
            return new Exception(exp.Message);
        }


        private bool hasInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead(GalenApiUrl + internetCheckUrl))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task GetProfile(Action<UserAccountInformation> onSuccessCallback, Action<Exception> onFailureCallback = null)
        {
            if (onSuccessCallback == null)
            {
                return;
            }
            try
            {
                checkAccess(true);

                // gets the current users profile
                User user = await Sdk.GetUser(currentUserId);

                if (user != null)
                {
                    UserAccountInformation userInfo = new UserAccountInformation();
                    userInfo.Email = user.emailAddress;
                    userInfo.FirstName = user.firstName;
                    userInfo.LastName = user.lastName;
                    //userInfo.MobilePhoneNumber = user.contactInfo.primaryPhone;
                    onSuccessCallback(userInfo);
                }
                throw new Exception("could-not-get-user-profile");
            }
            catch (Exception exp)
            {
                onFailureCallback?.Invoke(getCorrectException(exp));
            }
        }

        public async Task SendData(string serialNumber, string hrdFilePath, string lrdFilePath, string eventFilePath, string settingsFilePath, Action onSuccessCallback, Action<Exception> onFailureCallback = null)
        {

            if (onSuccessCallback == null)
            {
                return;
            }
            try
            {
                checkAccess(false);

                string filename = Path.GetFileNameWithoutExtension(lrdFilePath);
                string timestampStr = filename.Substring(0, filename.IndexOf("L"));
                DateTime timestamp = DateTime.Parse("yyyyMMdd'_'HHmmss");

                // sends data to the cloud

                // First post the serial number
                DeviceData basicData = new DeviceData();
                basicData.data = new Dictionary<string, object>();
                basicData.data.Add(PropertySerialNumber, serialNumber);
                basicData.data.Add(PropertyFileRecordTimeStamp, timestamp.ToString("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'"));
                string deviceDataId = await Sdk.SaveData(DeviceIdBPM, DevicePropertySetIdBPMFile, basicData);
                Boolean sendSucess = false;
                if (deviceDataId != null)
                {
                    sendSucess = true;
                    // zip and then post the file
                    string zipFilePath = "";
                    using (FileStream ms = new FileStream(zipFilePath, FileMode.Create))
                    {
                        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, false))
                        {
                            if (hrdFilePath != null)
                            {
                                var zipArchiveHrd = archive.CreateEntryFromFile(hrdFilePath, Path.GetFileName(hrdFilePath), CompressionLevel.Fastest);
                            }
                            var zipArchiveLrd = archive.CreateEntryFromFile(lrdFilePath, Path.GetFileName(lrdFilePath), CompressionLevel.Fastest);
                            var zipArchiveEvt = archive.CreateEntryFromFile(eventFilePath, Path.GetFileName(eventFilePath), CompressionLevel.Fastest);
                            var zipArchiveStf = archive.CreateEntryFromFile(settingsFilePath, Path.GetFileName(settingsFilePath), CompressionLevel.Fastest);
                        }
                    }

                    string updatedId = await Sdk.SaveData(deviceDataId, DeviceIdBPM, PropertyHrfFile, zipFilePath);
                    sendSucess = sendSucess && updatedId != null;

                    File.Delete(zipFilePath);

                    /*string updatedId = await Sdk.SaveData(deviceDataId, DeviceIdAirbox, PropertyHrfFile, hrdFilePath);
                    sendSucess = sendSucess && updatedId != null;

                    updatedId = await Sdk.SaveData(deviceDataId, DeviceIdAirbox, PropertyLrfFile, lrdFilePath);
                    sendSucess = sendSucess && updatedId != null;

                    updatedId = await Sdk.SaveData(deviceDataId, DeviceIdAirbox, PropertyEvfFile, eventFilePath);
                    sendSucess = sendSucess && updatedId != null;

                    updatedId = await Sdk.SaveData(deviceDataId, DeviceIdAirbox, PropertyStfFile, settingsFilePath);
                    sendSucess = sendSucess && updatedId != null;*/
                }
                if (!sendSucess)
                {
                    throw new Exception("could-not-send-airbox-data");
                }

                onSuccessCallback();
            }
            catch (Exception exp)
            {
                onFailureCallback?.Invoke(getCorrectException(exp));
            }
        }

        public async Task CreateAccount(UserAccountInformation user, Action onSuccessCallback, Action onFailureCallback)
        {
            if (onSuccessCallback == null)
            {
                return;
            }
            try
            {
                checkAccess(false);

                // creates an account
                User registerUser = new User();
                registerUser.firstName = user.FirstName;
                registerUser.lastName = user.LastName;
                registerUser.emailAddress = user.Email;
                //registerUser.dateOfBirth = user.DoB.ToString("yyyy-MM-dd");

                string message = await Sdk.RegisterUser(registerUser, user.Password, user.Password, TimeZoneInfo.Local.StandardName);
                if (message == null || !message.ToLower().StartsWith("registered user"))
                {
                    throw new Exception("could-not-create-account");
                }

                onSuccessCallback();
            }
            catch (Exception exp)
            {
                if (onFailureCallback == null)
                    getCorrectException(exp);
            }
        }

        public async Task Login(string emailAddress, string password, Action<UserAccountInformation> onSuccessCallback, Action<Exception> onFailureCallback = null)
        {
            if (onSuccessCallback == null)
            {
                return;
            }
            try
            {
                checkAccess(false);

                // logs in the current user
                UserAccountInformation userInfo = new UserAccountInformation();

                User user = await Sdk.Login(emailAddress, password);
                if (user != null)
                {
                    currentUserId = user.userId;

                    userInfo.Email = user.emailAddress;
                    userInfo.FirstName = user.firstName;
                    userInfo.LastName = user.lastName;
                    //userInfo.MobilePhoneNumber = user.contactInfo.primaryPhone;
                }
                onSuccessCallback(userInfo);
            }
            catch (Exception exp)
            {
                onFailureCallback?.Invoke(getCorrectException(exp));
            }
        }

        public async Task Logout(Action onSuccessCallback, Action<Exception> onFailureCallback = null)
        {
            if (onSuccessCallback == null)
            {
                return;
            }
            try
            {
                currentUserId = null;

                onSuccessCallback();
            }
            catch (Exception exp)
            {
                onFailureCallback?.Invoke(getCorrectException(exp));
            }
        }

        public async Task ChangePassword(string oldPassword, string newPassword, Action onSuccessCallback, Action onFailureCallback)
        {
            if (onSuccessCallback == null)
            {
                return;
            }
            try
            {
                checkAccess(true);
                // change password
                UpdatePasswordRequestBody updateRequest = new UpdatePasswordRequestBody(currentUserId, oldPassword, newPassword, newPassword);
                // await Sdk.UpdateUserPassword(updateRequest);

                onSuccessCallback();
            }
            catch (Exception exp)
            {
                if (onFailureCallback == null)
                    return;
            }
        }



        /*
        async void GalenPushData(DeviceData dataToSend, string propertyCode, int value, string deviceID, string devicePSetID)
        {
            //DeviceData dataToSend = new DeviceData();
            dataToSend.data = new Dictionary<string, object>();
            dataToSend.data.Add(( ( propertyCode) ), ( ( value) ));
            string deviceDataId = await Sdk.SaveData(((deviceID)), ((devicePSetID)), dataToSend);
        }
        */
    }
}
