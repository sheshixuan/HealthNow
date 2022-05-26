using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Food.Models;

namespace Food.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default

        private Database1Entities2 db = new Database1Entities2();

        public ActionResult Index(string Food)
        {
            if (Session["Name"] is null)
            {
                ViewBag.user = "gg";
            }
            else
            {
                string value = Session["Name"].ToString();
                ViewBag.user = value;
            }
            if (Food != null)
            {
                var count = db.FoodOutput.Where(x => x.Food == Food);
                if (count.Count() > 0)
                {
                    var Calories = db.FoodOutput.Where(x => x.Food == Food).Select(x => x.Calories).First();
                    ViewBag.Serving = db.FoodOutput.Where(x => x.Food == Food).Select(x => x.Serving).First();
                    ViewBag.Food = db.FoodOutput.Where(x => x.Food == Food).Select(x => x.Food).First();
                    ViewBag.Calories = db.FoodOutput.Where(x => x.Food == Food).Select(x => x.Calories).First();
                    ViewBag.cli = "0";
                    Session["Calories"] = Calories;
                    Session["Serving"] = ViewBag.Serving;
                    Session["Food"] = Food;
                }
                else
                {
                    ViewBag.cli = "1";

                }
            }

            ViewBag.FoodTable = db.FoodTable.ToList();

            var Foodtable = db.FoodTable.ToList();
            if (Foodtable.Count() > 0)
            {
                var sum = 0;
                foreach(var item in ViewBag.FoodTable)
                    {
                    if (item.Fooduser == ViewBag.user)

                    {

                        sum += item.Calores;

                        ViewBag.sum = sum;

                    }
                }
            } 

            return View();
        }

        [HttpPost]
        public ActionResult add(FoodTable fo, string Calories, string Food, string sl, string Serving, DateTime data)
        {

            
            string wl = HttpUtility.UrlDecode("*", Encoding.UTF8);
            Serving = Session["Serving"].ToString();
            Food = Session["Food"].ToString() + Serving + wl + sl;
            Calories = Session["Calories"].ToString();
            int sum = Convert.ToInt32(Calories) * Convert.ToInt32(sl);
            fo.Food = Food;
            fo.Calores = sum;
            fo.Serving = "1 posteiio";
            fo.Fooddata = data;
            fo.Fooduser = Session["Name"].ToString();
            db.FoodTable.Add(fo);
            db.SaveChanges();
            return Content("0");
        }

        //user index date selection
        public ActionResult addDate(FoodTable fo,DateTime data)
        {


            string wl = HttpUtility.UrlDecode("*", Encoding.UTF8);
            //Serving = Session["Serving"].ToString();
            //Food = Session["Food"].ToString() + Serving + wl + sl;
            //Calories = Session["Calories"].ToString();
            //int sum = Convert.ToInt32(Calories) * Convert.ToInt32(sl);
            ////fo.Food = Food;
            //fo.Calores = sum;
            ////fo.Serving = "1 posteiio";
            fo.Fooddata = data;
            //fo.Fooduser = Session["Name"].ToString();
            //db.FoodTable.Add(fo);
            db.SaveChanges();
            return Content("0");
        }
        public JsonResult Getfood(string search)

        {

            List<foodpre> allsearch = db.FoodOutput.Where(x => x.Food.Contains(search)).Select(x => new foodpre
            {
                ID = x.ID,
                Food = x.Food
                
            }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Loign() {
            return View();
        }
        public ActionResult LoignOut()
        {
            Session.Abandon();
            return Redirect("Loign");
        }
        [HttpPost]
        public ActionResult Loign(string Name,string PWD) {
            var data = db.FoodUser.Where(x=>x.UserName== Name).Where(x=>x.Userpwd==PWD).ToList();

            if (Name is null)
            {
                TempData["js"] = MvcHtmlString.Create("alert('Please input the username')");
                return View();
            }

            if (PWD is null)
            {
                TempData["js"] = MvcHtmlString.Create("alert('Please input the password')");
                return View();
            }


            if (data.Count()<=0)
            {
                TempData["js"] = MvcHtmlString.Create("alert('Incorrect username or password')");
                return View();
            }
            Session["Name"] = Name;
            Session["UserId"] = data.First().userBYid;
            return RedirectToAction("Home", "Default");
        }

        public ActionResult adduser() {
            return View();
        }

        [HttpPost]
        public ActionResult adduser(FoodUser fo, string Name, string PWD)
        {
            if (Name is "")
            {
                TempData["js"] = MvcHtmlString.Create("alert('Please input the username')");
                return View();
            }

            if (PWD is "")
            {
                TempData["js"] = MvcHtmlString.Create("alert('Please input the password')");
                return View();
            }

            var data = db.FoodUser.Where(x => x.UserName == Name).ToList();

            if (data.Count() > 0)
            {
                TempData["js"] = MvcHtmlString.Create("alert('The account is already registered')");
                return View();
            }
            fo.UserName = Name;
            fo.Userpwd = PWD;
            db.FoodUser.Add(fo);
            db.SaveChanges();
            var newdata = db.FoodUser.Where(q => q.UserName == Name).First();
            Session["Name"] = Name;
            Session["UserId"] = newdata.userBYid;
            return RedirectToAction("Home", "Default");
        }

        public ActionResult delete(int ID)
        {
            FoodTable fd = db.FoodTable.Find(ID);
            db.FoodTable.Remove(fd);
            db.SaveChanges();
            return RedirectToAction("Userindex", "Default");
        }

        public ActionResult deleteIndex(int ID)
        {
            FoodTable fd = db.FoodTable.Find(ID);
            db.FoodTable.Remove(fd);
            db.SaveChanges();
            return RedirectToAction("Index", "Default");
        }
        
        
        public ActionResult Userindex(DateTime? date)
        {

            if (date == null) date = DateTime.Now;
            string value = Session["Name"].ToString();
            int UserId = int.Parse(Session["UserId"].ToString());
            ViewBag.user = value;
            var FoodTable= db.FoodTable.SqlQuery($"select * from FoodTable where Fooduser='{value}' and Fooddata = '{date}'").ToList();
            ViewBag.SquatExercise = db.SquatExercise.SqlQuery($"select * from SquatExercise where UserId={UserId} and CONVERT(varchar(30),CreateTime,111) = '{date?.ToString("yyyy/MM/dd")}'").ToList();
            ViewBag.LegExercise = db.LegExercise.SqlQuery($"select * from LegExercise where UserId={UserId} and CONVERT(varchar(30),CreateTime,111) = '{date?.ToString("yyyy/MM/dd")}'").ToList();
            ViewBag.KnowUrBody = db.KnowUrBody.Where(q => q.UserId == UserId ).ToList();
            //ViewBag.SquatExercise1 = db.SquatExercise.Where(q => q.CreateTime == createTime).ToList();
            //ViewBag.SquatExercise = db.SquatExercise.Where(q=>q.UserId == UserId).ToList();
            var SquatSumNum = db.SquatExercise.Where(q => q.UserId == UserId).ToList();
            if (SquatSumNum.Count() > 0)
            {
                ViewBag.sumSquat = db.SquatExercise.SqlQuery($"select * from SquatExercise where UserId={UserId} and CONVERT(varchar(30),CreateTime,111) = '{date?.ToString("yyyy/MM/dd")}'").Sum(x => x.Caluries);
            }

            var LegSumNum = db.LegExercise.Where(q => q.UserId == UserId).ToList();
            //var FoodSumNum = db.FoodTable.Where(q => q.ID == UserId).ToList();
            //if (FoodSumNum.Count() > 0)
            //{
            ViewBag.sumFood = db.FoodTable.SqlQuery($"select * from FoodTable where Fooduser='{value}' and Fooddata = '{date}'").Sum(x => x.Calores);
            //}
            ViewBag.FoodTable = db.FoodTable.ToList();
            double target = 2000;
            double indxxxx = 100;
            double sumsq = Decimal.ToDouble(ViewBag.sumSquat);
            double process = ((ViewBag.sumFood - sumsq) / target) * indxxxx;
            ViewBag.process = process;
            var Foodtable = db.FoodTable.ToList();
            if (Foodtable.Count() > 0)
            {
                var sum = 0;
                foreach (var item in ViewBag.FoodTable)
                {
                    if (item.Fooduser == ViewBag.user)

                    {

                        sum += item.Calores;

                        ViewBag.sumindex = sum;

                        

                    }
                }
            }

            

            return View(FoodTable);
            

            }
        
       

        public ActionResult Home()
        {
            if (Session["Name"] is null)
            {
                ViewBag.user = "gg";
            }
            else
            {
                string value = Session["Name"].ToString();
                ViewBag.user = value;
            }

            return View();
        }

        public ActionResult Detection()
        {
            if (Session["Name"] is null)
            {
                ViewBag.user = "gg";
            }
            else
            {
                string value = Session["Name"].ToString();
                ViewBag.user = value;
            }

            return View();
        }

        public ActionResult Lifestyle()
        {
            if (Session["Name"] is null)
            {
                ViewBag.user = "gg";
            }
            else
            {
                string value = Session["Name"].ToString();
                ViewBag.user = value;
            }

            return View();
        }

        public ActionResult Exercise()
        {
            if (Session["Name"] is null)
            {
                ViewBag.user = "gg";
            }
            else
            {
                string value = Session["Name"].ToString();
                ViewBag.user = value;
            }

            return View();
        }

        public ActionResult Body()
        {
            if (Session["Name"] is null)
            {
                ViewBag.user = "gg";
                //return RedirectToAction("loign", "default");
            }
            else
            {
                string value = Session["Name"].ToString();
                ViewBag.user = value;
            }

            return View();
        }
        public ActionResult Squat()
        {
            if (Session["Name"] is null)
            {
                ViewBag.user = "gg";
                
            }
            else
            {
                string value = Session["Name"].ToString();
                ViewBag.user = value;
            }

            return View();
        }

        public ActionResult Leg()
        {
            if (Session["Name"] is null)
            {
                ViewBag.user = "gg";
                
            }
            else
            {
                string value = Session["Name"].ToString();
                ViewBag.user = value;
            }

            return View();
        }
        public ActionResult Posture()
        {
            if (Session["Name"] is null)
            {
                ViewBag.user = "gg";
            }
            else
            {
                string value = Session["Name"].ToString();
                ViewBag.user = value;
            }

            return View();
        }
        public ActionResult InsSquatExercise(int count, decimal caluries)
        {
            string value=string.Empty;
            value = Session["UserId"].ToString();
            
            SquatExercise squatExercise = new SquatExercise();
            squatExercise.Id = Guid.NewGuid().ToString();
            squatExercise.Count = count;
            squatExercise.Caluries = caluries;
            squatExercise.CreateTime = DateTime.Now;
            squatExercise.UserId =int.Parse(value);
            db.SquatExercise.Add(squatExercise);
            db.SaveChanges();
            return Json(true);
        }

        public ActionResult deleteSquat(string Id)
        {
            SquatExercise sq = db.SquatExercise.Find(Id);
            db.SquatExercise.Remove(sq);
            db.SaveChanges();
            return RedirectToAction("Userindex", "Default");
           
        }

        public ActionResult deleteLeg (string Id)
        {
           
           
            LegExercise lg = db.LegExercise.Find(Id);
            db.LegExercise.Remove(lg);
            db.SaveChanges();
            return RedirectToAction("Userindex", "Default");
            
        }

        public ActionResult deleteBody(string Id)
        {
           
            KnowUrBody bd = db.KnowUrBody.Find(Id);
            db.KnowUrBody.Remove(bd);
            db.SaveChanges();
            return RedirectToAction("Userindex", "Default");
           
           
        }
        public ActionResult InsLegExercise(string count, string caluries)
        {
            string value = Session["UserId"].ToString();
            LegExercise legExercise = new LegExercise();
            legExercise.Id = Guid.NewGuid().ToString();
            legExercise.Points = count;
            legExercise.Caluries = caluries;
            legExercise.CreateTime= DateTime.Now;
            legExercise.UserId =int.Parse(value);
            db.LegExercise.Add(legExercise);
            db.SaveChanges();
            return Json(true);
        }
        public ActionResult InsKnowUrBody(string gender, string ageGroup,string lifestyle,string bodyType,string result)
        {
            int value =int.Parse(Session["UserId"].ToString());
            var data=db.KnowUrBody.Where(x => x.UserId == value).ToList();
            if (data.Count==0) { 
            KnowUrBody knowUrBody = new KnowUrBody();
            knowUrBody.Id = Guid.NewGuid().ToString();
            knowUrBody.Gender = gender;
            knowUrBody.AgeGroup = ageGroup;
            knowUrBody.Lifestyle = lifestyle;
            knowUrBody.BodyType = bodyType;
            knowUrBody.Result = result;
                knowUrBody.UserId = value;
                db.KnowUrBody.Add(knowUrBody);
            db.SaveChanges();
            }
            else
            {
                data.First().Gender = gender;
                data.First().AgeGroup = ageGroup;
                data.First().Lifestyle = lifestyle;
                data.First().BodyType = bodyType;
                data.First().Result = result;
                data.First().UserId = value;
                db.SaveChanges();
            }
            return RedirectToAction("Userindex", "Default");
            ;
        }

        
    }

}