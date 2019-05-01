using System;
using System.Collections.Generic;
using System.Text;
using Plugin.GalenPCL;


namespace MoniHealth
{
    class GalenMethods
    {
        //Plugin.GalenPCL.DataOperator comm;
        //Plugin.GalenPCL.Device comm;
        //Plugin.GalenPCL.ContactInfo comm;
        //Plugin.GalenPCL.GalenStandardImplementation comm;
        //Plugin.GalenPCL.ActivateUserRequest comm;
        //DataOperator comm = new DataOperator();

        DeviceData dataToSend = new DeviceData();
        IGalenPCL Sdk;
        //GalenStandardImplementation comm = new GalenStandardImplementation();
        //DataOperator comm = new DataOperator();
        string TenantDomain = "WIlliam.Winter@galendata.com";


        async void GalenInitiateSDK()
        {
            //int init = await comm.Sdk.Init("https://test-ca.galencloud.com", TenantDomain);
            int init = await Sdk.Init("https://test-ca.galencloud.com", TenantDomain);
            int retry = 0;
            while (init != 0 && retry < 3)
            {
                //init = await comm.Sdk.Init("https://test-ca.galencloud.com", "ca-test.galencloud.com");
                init = await Sdk.Init("https://test-ca.galencloud.com", "ca-test.galencloud.com");
                retry++;
            }
        }
        
        async void GalenPushData(DeviceData dataToSend, string propertyCode, int value, string deviceID, string devicePSetID)
        {
            //DeviceData dataToSend = new DeviceData();
            dataToSend.data = new Dictionary<string, object>();
            dataToSend.data.Add(( ( propertyCode) ), ( ( value) ));
            string deviceDataId = await Sdk.SaveData(((deviceID)), ((devicePSetID)), dataToSend);
        }
        async void GalenPullData(string propertyCode, int value, string deviceID, string devicePSetID)
        {
            // deviceCriteria.Add(new DeviceCriteria({{PROPERTY_CODE}}, DataOperator.GreaterThanOrEqual, start.ToString("yyyy-MM-dd")));
            // deviceCriteria.Add(new DeviceCriteria({{PROPERTY_CODE}}, DataOperator.LessThanOrEqual, end.ToString("yyyy-MM-dd")));

        }
    }
}
