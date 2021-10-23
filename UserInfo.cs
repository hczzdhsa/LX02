using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL
{
    public class UserInfo
    {
        private UserInfo() { }
        private static UserInfo _instance = new UserInfo();
        public static UserInfo Instance
        {
            get
            {
                return _instance;
            }
        }
        string cns = AppConfigurtaionServices.Configuration.GetConnectionString("cns");
        public string UserCheck(string UserName)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select password from userinfo where username=@username;";
                return cn.ExecuteScalar<string>(sql, new { username = UserName });
            }
        }
        public Model.UserInfo GetModel(string UserName)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select * from userinfo where where username=@username;";
                return cn.QueryFirstOrDefault<Model.UserInfo>(sql, new { username = UserName });
            }
        }
        public IEnumerable<Model.UserInfo> GetAll()
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select * from userinfo";
                return cn.Query<Model.UserInfo>(sql);
            }
        }
        public int GetCount()
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select count(1) from userinfo";
                return cn.ExecuteScalar<int>(sql);
            }
        }
        public IEnumerable<Model.UserInfoNo> GetPage(Model.Page page)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "with a as(select row_number() over(order by username) as num, userinfo.* from userinfo)";
                sql += "select from a where num between (@pageIndex-1)*@pageSize+1 and @pageIndex*@pageSize;";
                return cn.Query<Model.UserInfoNo>(sql, page);
            }
        }
        public int Add(Model.UserInfo user)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "insert info userinfo values(@userName,@passWord,@qq,@email,@type,@userImg);";
                return cn.Execute(sql, user);
            }
        }
        public int Update(Model.UserInfo user)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "update userinfo set password=@passWord,qq=@qq,email=@email,type=@type,userImg=@userImg where username=@userName";
                return cn.Execute(sql, user);
            }
        }
        public int Delete(string username)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "delete from userinfo where username=@userName";
                return cn.Execute(sql, new { username = username });
            }
        }
    }
}