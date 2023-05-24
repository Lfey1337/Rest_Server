using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rest_Server.DTOs;
using Rest_Server.Services.DB_Models;
using System.Collections.Generic;
using Route = Rest_Server.DTOs.Route;

namespace Rest_Server.Services
{
    public class DBWorker
    {
        #region Comments
        public IEnumerable<Comment> GetComments()
        {
            var db = new SallySPContext();
            IEnumerable<Comment> comments = from u in db.Users
                                            join c in db.Comments on u.Id equals c.UserId
                                            join a in db.Avatars on u.Id equals a.Id
                                            select new Comment
                                            {
                                                Id = c.Id,
                                                UserId = u.Id,
                                                Nickname = u.Nickname,
                                                Rate = c.Rate,
                                                Avatar = !a.Img.IsNullOrEmpty() ? (File.Exists($@"{Directory.GetCurrentDirectory()}/images/avatars/{a.Img}.{a.Extension.Trim()}") ? Convert.ToBase64String(File.ReadAllBytes($@"{Directory.GetCurrentDirectory()}/images/avatars/{a.Img}.{a.Extension.Trim()}")) : "0") : "0", 
                                                Extension = a.Extension.Trim(),
                                                ComDescription = c.ComDescription
                                            };
            return comments;
        }
        public void InsertComment(int userId, string comDescription, double rate)
        {
            using var db = new SallySPContext();
            var comment = new Comments
            {
                UserId = userId,
                ComDescription = comDescription,
                Rate = rate,
            };
            db.Comments.Add(comment);
            db.SaveChanges();
        }
        public void DeleteComment(int userId, int commentId) 
        {
            using var db = new SallySPContext();
            var comment = db.Comments.Where(c => c.UserId == userId && c.Id == commentId).ExecuteDelete();
            db.SaveChanges();
        }
        public void UpdateComment(int commentId, int userId, string comDescription, double rate)
        {
            using var db = new SallySPContext();
            var comment = db.Comments.Where(c => c.UserId == userId && c.Id == commentId).ExecuteUpdate(c => c.SetProperty(r => r.Rate, r => rate).SetProperty(d => d.ComDescription, d => comDescription));
        }
        #endregion

        #region RouteComments
        public IEnumerable<Comment> GetRouteComments(int routeId)
        {
            var db = new SallySPContext();
            IEnumerable<Comment> comments = from u in db.Users
                                            join rc in db.RouteComments on u.Id equals rc.UserId
                                            join a in db.Avatars on u.Id equals a.Id
                                            join r in db.Routes on rc.RouteId equals r.Id
                                            where r.Id == routeId
                                            select new Comment
                                            {
                                                Id = rc.Id,
                                                UserId = u.Id,
                                                Nickname = u.Nickname,
                                                Rate = rc.Rate,
                                                Avatar = !a.Img.IsNullOrEmpty() ? (File.Exists($@"{Directory.GetCurrentDirectory()}/images/avatars/{a.Img}.{a.Extension.Trim()}") ? Convert.ToBase64String(File.ReadAllBytes($@"{Directory.GetCurrentDirectory()}/images/avatars/{a.Img}.{a.Extension.Trim()}")) : "0") : "0",
                                                Extension = a.Extension.Trim(),
                                                ComDescription = rc.ComDescription
                                            };
            return comments;
        }
        public void InsertRouteComment(int routeId, int userId, string comDescription, double rate)
        {
            using var db = new SallySPContext();
            var comment = new RouteComments
            {
                RouteId = routeId,
                UserId = userId,
                ComDescription = comDescription,
                Rate = rate,
            };
            db.RouteComments.Add(comment);
            db.SaveChanges();
        }
        public void DeleteRouteComment(int routeId, int userId, int commentId)
        {
            using var db = new SallySPContext();
            var comment = db.RouteComments.Where(c => c.UserId == userId && c.Id == commentId && c.RouteId == routeId).ExecuteDelete();
            db.SaveChanges();
        }
        public void UpdateRouteComment(int routeId, int userId, int commentId, string comDescription, double rate)
        {
            using var db = new SallySPContext();
            var comment = db.RouteComments.Where(c => c.UserId == userId && c.Id == commentId && c.RouteId == routeId).ExecuteUpdate(c => c.SetProperty(r => r.Rate, r => rate).SetProperty(d => d.ComDescription, d => comDescription));
        }
        #endregion

        #region Auth&User
        public bool GetAuth(string login, string password)
        {
            bool auth = false;
            var db = new SallySPContext();
            IEnumerable<Users> user = from u in db.Users
                                      join p in db.Passwords on u.Id equals p.Id
                                      where u.Nickname == login.ToLower()
                                      where p.Password == password
                                      select new Users
                                      {
                                          Id = u.Id
                                      };
            if (user.Any())
                auth = true;

            return auth;
        }
        public void CreateUser(string login, string password) 
        {
            int userId = GetLastUserId() + 1;
            using var db = new SallySPContext();
            var avatar = new Avatars
            {
                Id = userId,
                Img = "avatar1",
                Extension = "jpg"
            };
            var pass = new Passwords
            {
                Id = userId,
                Password = password
            };
            var user = new Users
            {
                Id = userId,
                Nickname = login,
                Mail = "Эл. почта",
                Phone = "Номер телефона"
            };
            db.Avatars.Add(avatar);
            db.Passwords.Add(pass);
            db.Users.Add(user);
            db.SaveChanges();
        }
        public UserPage GetUser(int userId)
        {
            var db = new SallySPContext();
            UserPage userPage;
            IEnumerable <UserPage> userPages = from u in db.Users
                                               join a in db.Avatars on u.Id equals a.Id
                                               where u.Id == userId
                                               select new UserPage
                                               {
                                                   Id = u.Id,
                                                   Avatar = !a.Img.IsNullOrEmpty() ? (File.Exists($@"{Directory.GetCurrentDirectory()}/images/avatars/{a.Img}.{a.Extension.Trim()}") ? Convert.ToBase64String(File.ReadAllBytes($@"{Directory.GetCurrentDirectory()}/images/avatars/{a.Img}.{a.Extension.Trim()}")) : "0") : "0",
                                                   Extension = a.Extension.Trim(),
                                                   Nickname = u.Nickname,
                                                   Email = u.Mail,
                                                   Phone = u.Phone
                                               };

            userPage = userPages.Any() ? userPage = userPages.First() : userPage = userPages.FirstOrDefault();
            return userPage;
        }
        public void UpdateUser(int userId, string nickname, string mail, string phone)
        {
            using var db = new SallySPContext();
            var user = db.Users.Where(c => c.Id == userId).ExecuteUpdate(c => c.SetProperty(u => u.Nickname, u => nickname)
            .SetProperty(m => m.Mail, m => mail)
            .SetProperty(p => p.Phone, p => phone));
        }

        /// <summary>
        /// If login is busy then true, else false
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool CheckLogin(string login)
        {
            bool log = false;
            var db = new SallySPContext();
            IEnumerable<Users> user = from u in db.Users
                                      where u.Nickname == login.ToLower()
                                      select new Users
                                      {
                                          Id = u.Id
                                      };
            if (user.Any())
                log = true;

            return log;
        }
        public UserIsAuth GetUser(string login)
        {
            var db = new SallySPContext();
            UserIsAuth user;
            IEnumerable<UserIsAuth> users = from u in db.Users
                                           join a in db.Avatars on u.Id equals a.Id
                                           where u.Nickname == login.ToLower()
                                           select new UserIsAuth
                                           {
                                               Id = u.Id,
                                               Nickname = u.Nickname,
                                               Avatar = !a.Img.IsNullOrEmpty() ? (File.Exists($@"{Directory.GetCurrentDirectory()}/images/avatars/{a.Img}.{a.Extension.Trim()}") ? Convert.ToBase64String(File.ReadAllBytes($@"{Directory.GetCurrentDirectory()}/images/avatars/{a.Img}.{a.Extension.Trim()}")) : "0") : "0",
                                               Extension = a.Extension.Trim()
                                           };

            return user = users.Any() ? users.First() : users.FirstOrDefault();
        }

        #endregion

        #region Routes
        public IEnumerable<Route> GetRoutes(int userId)
        {
            var db = new SallySPContext();
            IEnumerable<Route> routes;
            if (userId > 0)
            {
                if (db.LikesAndVisits.Where(c => c.UserId == userId).Count() == db.Routes.Count())
                {
                    routes = from r in db.Routes
                             join t in db.TitleImageRoute on r.Id equals t.Id
                             join l in db.LikesAndVisits on r.Id equals l.RouteId
                             where l.UserId == userId
                             select new Route
                             {
                                 Id = r.Id,
                                 RouteTitle = r.RouteTitle,
                                 RouteDesc = r.RouteDesc,
                                 RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{t.Img}.{t.Extension.Trim()}")),
                                 Extension = t.Extension.Trim(),
                                 RouteRate = r.RouteRate,
                                 RouteReviews = r.RouteReviews,
                                 IsLiked = l.IsLiked,
                                 IsVisited = l.IsVisited
                             };
                }
                else
                {
                    for (int routeId = 1; routeId <= db.Routes.Count(); routeId++)
                        if (!GetLikedOrVisitedRoutesForUser(userId, routeId).Any())
                        {
                            var likesAndVisits = new LikesAndVisits
                            {
                                UserId = userId,
                                RouteId = routeId
                            };
                            db.LikesAndVisits.Add(likesAndVisits);
                            db.SaveChanges();
                        }

                    routes = from r in db.Routes
                             join t in db.TitleImageRoute on r.Id equals t.Id
                             join l in db.LikesAndVisits on r.Id equals l.RouteId
                             where l.UserId == userId
                             select new Route
                             {
                                 Id = r.Id,
                                 RouteTitle = r.RouteTitle,
                                 RouteDesc = r.RouteDesc,
                                 RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{t.Img}.{t.Extension.Trim()}")),
                                 Extension = t.Extension.Trim(),
                                 RouteRate = r.RouteRate,
                                 RouteReviews = r.RouteReviews,
                                 IsLiked = l.IsLiked,
                                 IsVisited = l.IsVisited
                             };
                }
            } 
            else
            {
                routes = from r in db.Routes
                         join t in db.TitleImageRoute on r.Id equals t.Id
                         select new Route
                         {
                             Id = r.Id,
                             RouteTitle = r.RouteTitle,
                             RouteDesc = r.RouteDesc,
                             RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{t.Img}.{t.Extension.Trim()}")),
                             Extension = t.Extension.Trim(),
                             RouteRate = r.RouteRate,
                             RouteReviews = r.RouteReviews
                         };
            }
            return routes;
        }

        public IEnumerable<RoutePage> GetRoute(int routeId, int userId)
        {
            var db = new SallySPContext();
            IEnumerable<RoutePage> route;

            if (userId > 0)
            {
                if (GetLikedOrVisitedRoutesForUser(userId, routeId).Count() == 0)
                {
                    var likesAndVisits = new LikesAndVisits
                    {
                        UserId = userId,
                        RouteId = routeId
                    };
                    db.LikesAndVisits.Add(likesAndVisits);
                    db.SaveChanges();
                }
                route = from r in db.Routes
                        join i in db.TitleImageRoute on r.Id equals i.Id
                        join l in db.LikesAndVisits on r.Id equals l.RouteId
                        join rc in db.RouteContent on r.Id equals rc.Id
                        where r.Id == routeId
                        where l.UserId == userId
                        select new RoutePage
                        {
                            Id = r.Id,
                            RouteTitle = r.RouteTitle,
                            RouteDesc = r.RouteDesc,
                            RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{i.Img}.{i.Extension.Trim()}")),
                            Extension = i.Extension.Trim(),
                            RouteRate = r.RouteRate,
                            IsLiked = l.IsLiked,
                            IsVisited = l.IsVisited,
                            Content = rc.RouteContent
                        };
            }
            else
            {
                route = from r in db.Routes
                        join i in db.TitleImageRoute on r.Id equals i.Id
                        join l in db.LikesAndVisits on r.Id equals l.RouteId
                        join rc in db.RouteContent on r.Id equals rc.Id
                        where r.Id == routeId
                        select new RoutePage
                        {
                            Id = r.Id,
                            RouteTitle = r.RouteTitle,
                            RouteDesc = r.RouteDesc,
                            RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{i.Img}.{i.Extension.Trim()}")),
                            Extension = i.Extension.Trim(),
                            RouteRate = r.RouteRate,
                            Content = rc.RouteContent
                        };
            }
            return route;
        }
        public IEnumerable<Route> GetSearchRoutes(string query)
        {
            var db = new SallySPContext();
            IEnumerable<Route> route;
            route = from r in db.Routes
                    join i in db.TitleImageRoute on r.Id equals i.Id
                    join rc in db.RouteContent on r.Id equals rc.Id
                    where EF.Functions.Like(r.RouteDesc, $"%{query}%")
                    || EF.Functions.Like(r.RouteTitle, $"%{query}%")
                    || EF.Functions.Like(rc.RouteContent, $"%{query}%")
                    select new Route
                    {
                        Id = r.Id,
                        RouteTitle = r.RouteTitle,
                        RouteDesc = r.RouteDesc,
                        RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{i.Img}.{i.Extension.Trim()}")),
                        Extension = i.Extension.Trim(),
                        RouteRate = r.RouteRate,
                        RouteReviews = r.RouteReviews
                    };
            return route;
        }
        #endregion

        #region LikedAndVisitedRoutes
        public IEnumerable<Route> GetLikedRoutes(int userId)
        {
            var db = new SallySPContext();
            IEnumerable<Route> routes = from r in db.Routes
                                        join t in db.TitleImageRoute on r.Id equals t.Id
                                        join l in db.LikesAndVisits on r.Id equals l.RouteId
                                        join u in db.Users on l.UserId equals u.Id
                                        where l.IsLiked == true
                                        where u.Id == userId
                                        select new Route
                                        {
                                            Id = r.Id,
                                            RouteTitle = r.RouteTitle,
                                            RouteDesc = r.RouteDesc,
                                            RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{t.Img}.{t.Extension.Trim()}")),
                                            Extension = t.Extension.Trim(),
                                            RouteRate = r.RouteRate,
                                            RouteReviews = r.RouteReviews,
                                            IsVisited = l.IsVisited,
                                            IsLiked = l.IsLiked
                                        };
            return routes;
        }

        public IEnumerable<Route> GetVisitedRoutes(int userId)
        {
            var db = new SallySPContext();
            IEnumerable<Route> routes = from r in db.Routes
                                        join t in db.TitleImageRoute on r.Id equals t.Id
                                        join l in db.LikesAndVisits on r.Id equals l.RouteId
                                        join u in db.Users on l.UserId equals u.Id
                                        where l.IsVisited == true
                                        where u.Id == userId
                                        select new Route
                                        {
                                            Id = r.Id,
                                            RouteTitle = r.RouteTitle,
                                            RouteDesc = r.RouteDesc,
                                            RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{t.Img}.{t.Extension.Trim()}")),
                                            Extension = t.Extension.Trim(),
                                            RouteRate = r.RouteRate,
                                            RouteReviews = r.RouteReviews,
                                            IsVisited = l.IsVisited,
                                            IsLiked = l.IsLiked
                                        };
            return routes;
        }

        public void PutLikedRoute(int userId, int routeId, bool like)
        {
            if (!GetLikedOrVisitedRoutesForUser(userId, routeId).Any())
            {
                using var db = new SallySPContext();
                var likesAndVisits = new LikesAndVisits
                {
                    UserId = userId,
                    RouteId = routeId,
                    IsLiked = true
                };
                db.LikesAndVisits.Add(likesAndVisits);
                db.SaveChanges();
            }
            else
            {
                using var db = new SallySPContext();
                var likesAndVisits = db.LikesAndVisits.Where(c => c.UserId == userId && c.RouteId == routeId)
                    .ExecuteUpdate(c => c.SetProperty(r => r.IsLiked, r => like));
            }
        }

        public void PutVisitedRoute(int userId, int routeId, bool visit)
        {
            if (GetLikedOrVisitedRoutesForUser(userId, routeId).Count() == 0)
            {
                using var db = new SallySPContext();
                var likesAndVisits = new LikesAndVisits
                {
                    UserId = userId,
                    RouteId = routeId,
                    IsVisited = true,
                    IsLiked = false,
                };
                db.LikesAndVisits.Add(likesAndVisits);
                db.SaveChanges();
            }
            else
            {
                using var db = new SallySPContext();
                var likesAndVisits = db.LikesAndVisits.Where(c => c.UserId == userId && c.RouteId == routeId)
                    .ExecuteUpdate(c => c.SetProperty(r => r.IsVisited, r => visit));
            }
        }
        #endregion

        #region WORK METHODS
        public IEnumerable<Route> GetLikedOrVisitedRoutesForUser(int userId, int routeId)
        {
            var db = new SallySPContext();
            IEnumerable<Route> routes = from r in db.Routes
                                        join t in db.TitleImageRoute on r.Id equals t.Id
                                        join l in db.LikesAndVisits on r.Id equals l.RouteId
                                        join u in db.Users on l.UserId equals u.Id
                                        where u.Id == userId
                                        where l.RouteId == routeId
                                        select new Route
                                        {
                                            Id = r.Id,
                                            RouteTitle = r.RouteTitle,
                                            RouteDesc = r.RouteDesc,
                                            RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{t.Img}.{t.Extension.Trim()}")),
                                            Extension = t.Extension.Trim(),
                                            RouteRate = r.RouteRate,
                                            RouteReviews = r.RouteReviews,
                                            IsVisited = l.IsVisited,
                                            IsLiked = l.IsLiked
                                        };
            return routes;
        }
        public IEnumerable<Route> GetAllLikedOrVisitedRoutesForUser(int userId)
        {
            var db = new SallySPContext();
            IEnumerable<Route> routes = from r in db.Routes
                                        join t in db.TitleImageRoute on r.Id equals t.Id
                                        join l in db.LikesAndVisits on r.Id equals l.RouteId
                                        join u in db.Users on l.UserId equals u.Id
                                        where u.Id == userId
                                        select new Route
                                        {
                                            Id = r.Id,
                                            RouteTitle = r.RouteTitle,
                                            RouteDesc = r.RouteDesc,
                                            RouteImg = Convert.ToBase64String(File.ReadAllBytes($"{Directory.GetCurrentDirectory()}/images/titleimagesroute/{t.Img}.{t.Extension.Trim()}")),
                                            Extension = t.Extension.Trim(),
                                            RouteRate = r.RouteRate,
                                            RouteReviews = r.RouteReviews,
                                            IsVisited = l.IsVisited,
                                            IsLiked = l.IsLiked
                                        };
            return routes;
        }

        public int GetLastUserId()
        {
            int userId = 0;
            var db = new SallySPContext();
            var user = from u in db.Users
                       select u.Id;
            userId = user.Max();
            return userId;
        }
        #endregion
    }
}