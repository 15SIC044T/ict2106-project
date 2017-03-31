using System;
using System.Web;
using BookStore.Models;
using System.IO;
using BookStore.DAL;

namespace BookStore.Controllers
{
    internal class ContentRepository
    {
        private ItemGateway itemGateway = new ItemGateway();
        private BookStoreContext db = new BookStoreContext();

        public ContentRepository()
        {

        }

        internal int UploadImageInDataBase(HttpPostedFileBase file, Item item)
        {
            //item.iImage = ConvertToBytes(file);
            var Content = new Item
            {
                iName = item.iName,
                iCategory = item.iCategory,
                iDescription = item.iDescription,
                iImage = item.iImage,
                iPrice = item.iPrice,
                iQuantity = item.iQuantity
            };

            itemGateway.Insert(Content);
            int i = db.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}