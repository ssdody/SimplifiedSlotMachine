namespace BedeSimplifiedSlotMachineTask.Providers
{
    using BedeSimplifiedSlotMachineTask.Providers.Contracts;
    using System.IO;

    public class ImageProvider : IImageProvider
    {
        private const string imagesPath = "../../../BedeSimplifiedSlotMachineTask.Providers/Images";

        public string GetImageLocation(string name)
        {
            var images = Directory.GetFiles(imagesPath);
            foreach (var img in images)
            {
                var imgSybol = img.Substring(img.LastIndexOf(".") - 1, 1);

                if (imgSybol == name)
                {
                    return img;
                }
            }
            return string.Empty;
        }
    }
}
