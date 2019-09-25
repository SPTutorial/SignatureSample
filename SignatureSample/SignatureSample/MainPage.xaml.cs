using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SignatureSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            Stream sigimage = await Padview.GetImageStreamAsync(SignatureImageFormat.Png);
            if (sigimage == null)
                return;
            BinaryReader br = new BinaryReader(sigimage);
            br.BaseStream.Position = 0;
            Byte[] All = br.ReadBytes((int)sigimage.Length);
            byte[] image = (byte[])All;

            bool storegaePermissionResp = await IsAvailStoragePermission();
            if (storegaePermissionResp)
            {
                var path = DependencyService.Get<IStorage>().SaveImage(image);
                Padview.Clear();
                DisplayAlert("Message", "Signature Saved", "Ok");
            }
            else
            {
                DisplayAlert("Message", "User denied to storage permission", "Ok");
            }
        }

        private async Task<bool> IsAvailStoragePermission()
        {

            bool response = false;
            PermissionStatus permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (permissionStatus != PermissionStatus.Granted)
            {
                var resp = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                if (resp.ContainsKey(Permission.Storage))
                {
                    permissionStatus = resp[Permission.Storage];
                }


                if (permissionStatus == PermissionStatus.Granted)
                {
                    response = true;
                }
                else if (permissionStatus == PermissionStatus.Denied)
                {
                    response = false;
                }
            }
            else
            {
                response = true;
            }
            return response;
        }
    }
}
