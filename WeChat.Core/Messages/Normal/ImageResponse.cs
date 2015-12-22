using System.Xml.Linq;

namespace WeChat.Core.Messages.Normal
{
    public class ImageResponse : RequestBase
    {
        public ImageModel Image { get; set; }

        public class ImageModel
        {
            public string MediaId { get; set; }
        }

        public override string ToXml()
        {
            if (this.Image == null) this.Image = new ImageModel();

            XElement ele = new XElement("xml",
                ModelBase.ToXmls(this),
                new XElement("Image", ToXmls<ImageModel>(this.Image)));

            return ele.ToString();
        }
    }
}
