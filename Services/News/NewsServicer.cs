using FOLYFOOD.Dto;
using FOLYFOOD.Dto.NewsDto;
using FOLYFOOD.Entitys;
using FOLYFOOD.Hellers.imageChecks;
using FOLYFOOD.Hellers;
using FOLYFOOD.Services.product;

namespace FOLYFOOD.Services.News
{
    public class NewsServicer
    {
        public readonly Context DBContext;

        public NewsServicer()
        {
            DBContext = new Context();
        }

        public async Task<RetunObject<FOLYFOOD.Entitys.News>> CrateNews(NewsRequest newsRequest)
        {
           Account account =  DBContext.Accounts.SingleOrDefault(x => x.AccountId == newsRequest.AccountId);
            try
            {
                if(account == null)
                {
                    throw new Exception("Không tồn tại tài khoảng. ");
                }
                if (account.DecentralizationId != 1)
                {
                    throw new Exception("Tài khoảng của bạn không có quyền được thêm Bài viết. ");
                }
                if(newsRequest.ShortDescription == null || newsRequest.Content == null || newsRequest.Title == null) {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin các trường");
                }
                bool checkImage = ImageChecker.IsImage(newsRequest.Image);

                if (!checkImage)
                {
                    throw new Exception("xử lý ảnh lỗi");
                }
            }
            catch (Exception ex)
            {
                return new RetunObject<FOLYFOOD.Entitys.News>()
                {
                    data = null,
                    mess = ex.Message,
                    statusCode = 404
                };
            }

            string link = await uplloadFile.UploadFile(newsRequest.Image,true);
            FOLYFOOD.Entitys.News newCreate = new FOLYFOOD.Entitys.News()
            {
                AccountId = account.AccountId,
                Title = newsRequest.Title,
                Content = newsRequest.Content,
                Image = link,
                ShortDescription = newsRequest.ShortDescription,
                IsShow = true

            };
            await DBContext.News.AddAsync(newCreate);
            await DBContext.SaveChangesAsync();
            return new RetunObject<FOLYFOOD.Entitys.News>()
            {
                data = newCreate,
                mess = "thêm dữ liệu thành công",
                statusCode = 201
            };
        }
    }
}
