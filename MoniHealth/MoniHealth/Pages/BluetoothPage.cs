using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE.Abstractions.EventArgs;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;


namespace MoniHealth.Pages
{
    public class BluetoothTestPage : ContentPage
    {
        Label texxt = new Label() { Text = "" };
        Label devicestr = new Label() { Text = "" };
        Label str = new Label() { Text = "" };
        IAdapter adapter;
        IBluetoothLE bluetoothBLE;
        ObservableCollection<IDevice> deviceList;
        IDevice device;
        IList<IService> Services;
        IService Service;
        IList<ICharacteristic> Characteristics;
        ICharacteristic Characteristic;
        IDescriptor descriptor;
        IList<IDescriptor> descriptors;


        public BluetoothTestPage()
        {
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
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            scanButton.Clicked += btnScan_Clicked;

            Button connectButton = new Button
            {
                Text = " Connect ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            connectButton.Clicked += btnConnect_Clicked;

            Button GetServicesButton = new Button
            {
                Text = "GetServices",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            GetServicesButton.Clicked += btnGetServices_Clicked;

            Button GetcharactersButton = new Button
            {
                Text = "Getcharacters",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            GetcharactersButton.Clicked += btnGetcharacters_Clicked;

            Button GetdescButton = new Button
            {
                Text = "desc",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            GetdescButton.Clicked += btnDescriptors_Clicked;








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

            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Test Page" },
                    scanButton,
                    //devicesList,
                    texxt,
                    connectButton,
                    GetServicesButton,
                    GetcharactersButton,
                    GetdescButton,
                    devicesListed,
                    //texxt
                    //str,
                    //devicestr,
                    //connectButton
                }
            };

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

                        /*foreach (var str1 in deviceList)
                        {
                            if (str1.Name != null)
                            {  }
                            str.Text = str.Text + str1.Name + "LOve";
                        }
                        devicestr.Text = "Done";*/
                        /*{
                            try
                            {
                                await adapter.ConnectToDeviceAsync(device);
                            }
                            catch (DeviceConnectionException)
                            {
                                await DisplayAlert(" Attention ", " Null device ", " OK ");
                            }
                            texxt.Text = texxt.Text + status.ToString() + "hi";
                        }*/
                    }
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again", "OK");
                }
            }

            async void btnConnect_Clicked(object sender, EventArgs e)
            {
                try
                {
                    if (device != null)
                    {
                        await adapter.ConnectToDeviceAsync(device);

                    }
                    else
                    {
                        await DisplayAlert("Notice", "No Device selected !", "OK");
                    }
                }
                catch (DeviceConnectionException ex)
                {
                    //Could not connect to the device
                    await DisplayAlert("Notice", ex.Message.ToString(), "OK");
                }
            }

            async void DevicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
            {
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
                }
                catch (DeviceConnectionException ex)
                {
                    await DisplayAlert(" Error ", ex.Message, " OK ");
                }
            }

            

            async void btnGetServices_Clicked(object sender, EventArgs e)
            {

                Services = await device.GetServicesAsync();
                // Service = await device.GetServiceAsync(Guid.Parse("guid")); 
                //or we call the Guid of selected Device

                Service = await device.GetServiceAsync(device.Id);

                foreach (var ser in Services)
                {
                    texxt.Text = texxt.Text + " \n" + ser.Name ;
                }
            }


            async void btnGetcharacters_Clicked(object sender, EventArgs e)
            {
                //Characteristics = await Services[0].GetCharacteristicsAsync();
                foreach (var ser in Services)
                {
                    Characteristics = await ser.GetCharacteristicsAsync();
                    foreach (var car in Characteristics)
                    {
                        //var idGuid = car.Id;
                        //Characteristic = await Service.GetCharacteristicAsync(idGuid);
                        //  Characteristic.CanRead
                        texxt.Text = texxt.Text + " \n" + car.Name;
                    }
                }
                /*foreach (var ser in Characteristics)
                {
                    texxt.Text = texxt.Text + " \n" + ser.Name;
                }*/
            }

            
            
            async void btnDescriptors_Clicked(object sender, EventArgs e)
            {
                foreach (var ser in Services)
                {
                    Characteristics = await ser.GetCharacteristicsAsync();
                    foreach (var car in Characteristics)
                    {
                        descriptors = await car.GetDescriptorsAsync();
                        foreach (var des in descriptors)
                        {
                            texxt.Text = texxt.Text + " \n" + des.Name;
                        }
                        

                    }
                }
                //descriptors = await Characteristic.GetDescriptorsAsync();






                //descriptor = await Characteristic.GetDescriptorAsync(Guid.Parse("guid"));

            }

















        }
    }
}