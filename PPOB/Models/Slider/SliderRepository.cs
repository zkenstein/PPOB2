﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mmt_Library;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PPOB.Models;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Http;

namespace PPOB.Models.Slider
{
    public class SliderRepository
    {
        dbAccess dbAccess = new dbAccess();
        string conn = ConfigurationManager.ConnectionStrings["GetConn"].ConnectionString;

        public async Task<List<MasterSlider>> GetSlider()
        {
            List<MasterSlider> Result = new List<MasterSlider>();
            try
            {
                string Query = "GetSlider";
                DataTable dt = new DataTable();
                dbAccess.strConn = conn;
                dt = await dbAccess.GetDataTable(Query);

                foreach (DataRow dr in dt.Rows)
                {
                    string path = HttpContext.Current.Server.MapPath(Convert.ToString(dr["Photo"]));                 
                    var ImageBase64 = GetBase64StringForImage(path);
                    Result.Add(
                        new MasterSlider
                        {
                            ID = Convert.ToString(dr["Id"]),
                            Photo = "data:image/png;base64," + ImageBase64,
                            Judul = Convert.ToString(dr["Judul"]),
                            DesSingkat = Convert.ToString(dr["DesSingkat"]),
                            DesPanjang = Convert.ToString(dr["DesPanjang"])
                        });
                }
                return Result;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Boolean> CreateSlider(string Photo, string Judul, string DesSingkat, string DesPanjang,string User)
        {
            Boolean Result = false;
            try
            {                

                string Query = "CreateSlider";
                dbAccess.strConn = conn;
                SqlParameter[] pParam = new SqlParameter[5];
                pParam[0] = new SqlParameter("@Photo", SqlDbType.VarChar);
                pParam[0].Value = Photo;
                pParam[1] = new SqlParameter("@Judul", SqlDbType.VarChar);
                pParam[1].Value = Judul;
                pParam[2] = new SqlParameter("@DesSingkat", SqlDbType.VarChar);
                pParam[2].Value = DesSingkat;
                pParam[3] = new SqlParameter("@DesPanjang", SqlDbType.VarChar);
                pParam[3].Value = DesPanjang;
                pParam[4] = new SqlParameter("@User", SqlDbType.VarChar);
                pParam[4].Value = User;

                int res = await dbAccess.ExecQueryByCommand(Query, pParam);
                if (res == 1)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
            catch (Exception ex)
            {
                Result = false;
                throw;
            }
            return Result;
        }

        public async Task<Boolean> EditSlider(string ID, byte[] Photo, string Judul, string DesSingkat, string DesPanjang)
        {
            Boolean Result = false;
            try
            {
                string Query = "EditSlider";
                dbAccess.strConn = conn;
                SqlParameter[] pParam = new SqlParameter[5];
                pParam[0] = new SqlParameter("@Id", SqlDbType.VarChar);
                pParam[0].Value = ID;
                pParam[1] = new SqlParameter("@Judul", SqlDbType.VarChar);
                pParam[1].Value = Judul;
                pParam[2] = new SqlParameter("@Photo", SqlDbType.VarChar);
                pParam[2].Value = Photo;
                pParam[3] = new SqlParameter("@DesSingkat", SqlDbType.VarChar);
                pParam[3].Value = DesSingkat;
                pParam[4] = new SqlParameter("@DesPanjang", SqlDbType.VarChar);
                pParam[4].Value = DesPanjang;
                int res = await dbAccess.ExecQueryByCommand(Query, pParam);
                if (res == 1)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
            catch (Exception ex)
            {
                Result = false;
                throw;
            }
            return Result;
        }

        public async Task<Boolean> DeleteSlider(string ID)
        {
            Boolean Result = false;
            try
            {
                string Query = "DeleteSlider";

                dbAccess.strConn = conn;
                SqlParameter[] pParam = new SqlParameter[1];
                pParam[0] = new SqlParameter("@Id", SqlDbType.VarChar);
                pParam[0].Value = ID;
                int res = await dbAccess.ExecQueryByCommand(Query, pParam);
                if (res == 1)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
            catch (Exception ex)
            {
                Result = false;
                throw;
            }
            return Result;
        }
      
        protected static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
    }
}