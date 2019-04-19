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
        Label texxt = new Label() { Text = ""};
        IAdapter adapter;
        IBluetoothLE bluetoothBLE;
        ObservableCollection<IDevice> deviceList;
        IDevice device;

        public BluetoothTestPage()
        {
            Title = "Blue";
            bluetoothBLE = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;

            deviceList = new ObservableCollection<IDevice>();

            Button scanButton = new Button
            {
                Text = "Scan",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            scanButton.Clicked += btnScan_Clicked;

            Button connectButton = new Button
            {
                Text = "Connect",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            connectButton.Clicked += btnConnect_Clicked;

            //ListView devicesList = new ListView
            //{
            //    ItemsSource = deviceList,
            //    IsPullToRefreshEnabled = true,
            //    ItemTemplate = new DataTemplate(() =>
            //    {
            //        Label nameLabel = new Label();
            //        nameLabel.SetBinding(Label.TextProperty, device.Name);

            //        Label addressLabel = new Label();
            //        addressLabel.SetBinding(Label.TextProperty, "test");

            //        return new ViewCell
            //        {
            //            View = new StackLayout
            //            {
            //                Padding = new Thickness(0, 5),
            //                Children =
            //                {
            //                nameLabel,
            //                //addressLabel
            //                }
            //            }
            //        };
            //    }),
            //};
            //deviceList.ItemSelected += DevicesList_OnItemSelected;

            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Test Page" },
                    scanButton,
                    //devicesList,
                    texxt,
                    connectButton
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
                        texxt.Text = status.ToString() + "hi";
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }/*
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
                        await adapter.StartScanningForDevicesAsync();
                        adapter.DeviceDiscovered += (obj, a) =>
                        {
                            if (!deviceList.Contains(a.Device))
                                deviceList.Add(a.Device);
                            // deviceList.Add(a.Device);
                        };
                        await adapter.StartScanningForDevicesAsync();
                        {
                            await adapter.ConnectToDeviceAsync(device);
                        }
                    }
                }
                else if (status != PermissionStatus.Unknown)
                    await DisplayAlert("Location Denied", "Can not continue, try again", "OK");*/
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
                //device = devicesList.SelectedItem as IDevice;

                var result = await DisplayAlert(" WARNING ", " Do you want to connect to this device? ", " Connect ", " Cancel ");

                if (!result)
                    return;

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
        }
    }
}