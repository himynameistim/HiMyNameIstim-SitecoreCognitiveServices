using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiMyNameIstim_SitecoreCognitiveServices
{
    public class ImageSavedHandler
    {
        public void OnItemSaved(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item savedItem = Event.ExtractParameter(args, 0) as Item;

            // Allow only non null items and allow only items from the master database
            if (savedItem != null && savedItem.Database.Name.ToLower() == "master")
            {
                // Limit templates to Jpeg

                if (savedItem.TemplateID == ID.Parse("{F1828A2C-7E5D-4BBD-98CA-320474871548}"))
                {
                    Media media = MediaManager.GetMedia(savedItem);
                    if (media != null)
                    {
                        MediaStream mediaStream = media.GetStream();
                        Byte[] bytes = new Byte[mediaStream.Length];
                        mediaStream.Stream.Read(bytes, 0, bytes.Length);

                        var analysisResults = CognitiveServicesHelper.MakeAnalysisRequest(bytes);


                        // Get the data that you need to populate here
                        //ImageField imageField = savedItem.Fields["Media"];
                        //MediaItem mediaItem = imageField.MediaItem;
                        //Stream stream = mediaItem.GetMediaStream();
                        //Byte[] bytes = new Byte[stream.Length];
                        //stream.Read(bytes, 0, bytes.Length);

                        //var analysisResults = CognitiveServicesHelper.MakeAnalysisRequest(bytes);

                        // Start Editing the Item

                        using (new SecurityDisabler())
                        {
                            savedItem.Editing.BeginEdit();

                            var result = analysisResults;

                            savedItem.Editing.EndEdit();
                        }
                    }
                }
            }
        }
    }
}
