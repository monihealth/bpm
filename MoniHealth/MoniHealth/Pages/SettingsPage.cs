using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE.Abstractions.Extensions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MoniHealth.Pages
{
    public class SettingsPage : ContentPage
    {
        App logger = new App();
        Label texxt = new Label() { Text = "" };
        Label devicestr = new Label() { Text = "" };
        Label str = new Label() { Text = "" };
        IAdapter adapter;
        IBluetoothLE bluetoothBLE;
        ObservableCollection<IDevice> deviceList;
        IDevice device;
        IList<AdvertisementRecord> recode;
        IList<IService> Services;
        IService Service;
        IList<ICharacteristic> Characteristics;
        ICharacteristic Characteristic;
        IDescriptor descriptor;
        IList<IDescriptor> descriptors;
        int pin = 513138;


        public SettingsPage()
        {
            #region Bluetooth Connection
            Title = "Blue";
            bluetoothBLE = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;

            deviceList = new ObservableCollection<IDevice>();
            //lv.ItemsSource = deviceList;


            Button scanButton = new Button
            {
                Text = " Scan ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            scanButton.Clicked += btnScan_Clicked;

            Button GetServicesButton = new Button
            {
                Text = "GetServices",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            //GetServicesButton.Clicked += btnGetServices_Clicked;


            ListView devicesListed = new ListView
            {
                ItemsSource = deviceList,
                VerticalOptions = LayoutOptions.Start,
                IsPullToRefreshEnabled = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    Label nameLabel = new Label() { };
                    nameLabel.SetBinding(Label.TextProperty, "Name");
                    //Label addressLabel = new Label();
                    //addressLabel.SetBinding(Label.TextProperty, "Id");
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            //Padding = new Thickness(0, 5),
                            Children =
                            {
                                nameLabel,
                                //addressLabel
                            }
                        }
                    };
                }),
            };
            devicesListed.ItemSelected += DevicesList_OnItemSelected;
            #endregion

            #region User info
            var Userinfo = new Label { Text = "User info", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), HorizontalOptions = LayoutOptions.Start };
            var NameLabel = new Label { Text = "Name: ", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var Name = new Entry
            {
                IsReadOnly = true,
                Text = "Brian Lee",
                FontSize = 13,
                Placeholder = "Enter email address",
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            var HeightLabel = new Label { Text = "Height: ", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var Height = new Entry
            {
                IsReadOnly = true,
                Text = "5'11''",
                FontSize = 13,
                Placeholder = "Enter email address",
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            var WeightLabel = new Label { Text = "Weight: ", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var Weight = new Entry
            {
                Text = "185",
                IsReadOnly = true,
                FontSize = 13,
                Placeholder = "Enter email address",
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            var EmailLabel = new Label { Text = "Email: ", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var Email = new Entry
            {
                Text ="Will42iam@gmail.com",
                IsEnabled = false,
                Keyboard = Keyboard.Email,
                FontSize = 13,
                Placeholder = "Enter email address",
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            var PasswordLabel = new Label { Text = "Password: ", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var Password = new Entry
            {
                Text = "asdf1234",
                IsReadOnly = true,
                Keyboard = Keyboard.Text,
                FontSize = 13,
                Placeholder = "Enter password",
                IsPassword = true,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            #endregion

            Button UpdateButton = new Button
            {
                Text = "  Update  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.Silver,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            UpdateButton.Clicked += UpdateClicked;
            Button Logout = new Button
            {
                Text = "  Logout  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.Silver,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            Logout.Clicked += LogoutChaAsync;

            Title = "Settings";
            Content = new StackLayout
            {
                Margin = new Thickness(20),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    //new Label { Text = "Settings Page" },
                    Userinfo,
                    new StackLayout(){ HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal, Children={NameLabel, Name}},
                    new StackLayout(){ HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal, Children={HeightLabel, Height }},
                    new StackLayout(){ HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal, Children={WeightLabel, Weight}},
                    new StackLayout(){ HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal, Children={EmailLabel, Email}},
                    new StackLayout(){ HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal, Children={PasswordLabel, Password}},
                    UpdateButton,
                    texxt,
                    Logout,
                    scanButton,
                    devicesListed
                }
            };


            void UpdateClicked(object sender, EventArgs e)
            {
                if (Name.IsReadOnly == true)
                {
                    Name.IsReadOnly = false;
                    Height.IsReadOnly = false;
                    Weight.IsReadOnly = false;
                    Password.IsReadOnly = false;
                }
                else if (Name.IsReadOnly == false)
                {
                    Name.IsReadOnly = true;
                    Height.IsReadOnly = true;
                    Weight.IsReadOnly = true;
                    Password.IsReadOnly = true;
                }


            }


        }

        void LogoutChaAsync(object sender, EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new LoginPage());
            MessagingCenter.Send<object>(this, App.EVENT_LAUNCH_LOGIN_PAGE);
        }

        async void btnScan_Clicked(object sender, EventArgs e)
        {
            //Request Location Permissions
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await DisplayAlert("Location permissions required for bluetooth", "Allow Permission", "OK");
                    //need to add a no option 
                }
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (results.ContainsKey(Permission.Location))
                {
                    status = results[Permission.Location];
                }
            }
            //If permissions were granted, scan for device
            if (status == PermissionStatus.Granted)
            {
                //If bluetooth is turned off...
                if (bluetoothBLE.State == BluetoothState.Off)
                {
                    await DisplayAlert(" Attention ", " Bluetooth disabled ", " OK ");
                }
                else
                {
                    deviceList.Clear();
                    //adapter.ScanTimeout = 10000;
                    adapter.ScanMode = ScanMode.Balanced;
                    //await adapter.StartScanningForDevicesAsync();
                    adapter.DeviceDiscovered += (obj, a) =>
                    {
                        if (!deviceList.Contains(a.Device) && a.Device.Name != null)
                        {
                            deviceList.Add(a.Device);

                        }
                        // deviceList.Add(a.Device);
                    };
                    /*foreach (var str1 in deviceList)
                    {
                        str.Text = str.Text + str1.Name; 
                    }
                    devicestr.Text = "Done";*/

                    if (!bluetoothBLE.Adapter.IsScanning)
                    {
                        await adapter.StartScanningForDevicesAsync();


                    }

                }
            }
            else if (status != PermissionStatus.Unknown)
            {
                await DisplayAlert("Location Denied", "Can not continue, try again", "OK");
            }
        }

        async void DevicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var devicesListed = (ListView)sender; 
            device = devicesListed.SelectedItem as IDevice;

            var result = await DisplayAlert(" WARNING ", " Do you want to connect to this device? ", " Connect ", " Cancel ");
            //texxt.Text = texxt.Text +"hi";
            if (!result)
            {
                //texxt.Text = texxt.Text + "hi2";
                return;

            }
            // Stop Scanner
            await adapter.StopScanningForDevicesAsync();

            try
            {
                await adapter.ConnectToDeviceAsync(device);

                await DisplayAlert(" Connected ", " Status: " + device.State, " OK ");

                btnGetServices_Clicked();
            }
            catch (DeviceConnectionException ex)
            {
                await DisplayAlert(" Error ", ex.Message, " OK ");
            }
            texxt.Text = device.State.ToString();
        }

        async void btnGetServices_Clicked()
        {

            Services = await device.GetServicesAsync();


            Characteristics = await Services[0].GetCharacteristicsAsync();

            texxt.Text = texxt.Text + " \n" + device.Name;

        }




    }
}