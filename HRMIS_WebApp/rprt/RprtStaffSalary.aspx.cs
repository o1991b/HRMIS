﻿using HRWebApp.cs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRWebApp.rprt
{
    public partial class RprtStaffSalary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["HRMIS_UserID"] == null || Session["HRMIS_UserData"] == null) Response.Redirect("~/login");
            else
            {
                setDatas();
            }
        }
        protected void setDatas()
        {
            CMain MainClass = new CMain();
            GetTableData myObjGetTableData = new GetTableData();
            DataTable dt = null;
            CSession sessionClass = new CSession();
            try
            {
                var userData = sessionClass.getCurrentUserData();
                bool boolRoleUser = false;
                for (int i = 0; i < userData.USR_ROLEDATA.Capacity; i++)
                {
                    if (userData.USR_ROLEDATA[i] == 1 || userData.USR_ROLEDATA[i] == 18)
                    {
                        boolRoleUser = true;
                        break;
                    }
                }
                if (!boolRoleUser) throw new cs.HRMISException("Тайлангийн хэсэгт хандах эрх байхгүй байна! Хандах эрхийг ТЗУГ-тай холбогдон авна уу.");
                dt = MainClass.getStaffSalaryYears();
                selectFilterTab1Year.DataSource = dt;
                selectFilterTab1Year.DataTextField = "YEAR";
                selectFilterTab1Year.DataValueField = "YEAR";
                selectFilterTab1Year.DataBind();
                selectFilterTab1Year.SelectedIndex = selectFilterTab1Year.Items.IndexOf(selectFilterTab1Year.Items.FindByValue(DateTime.Now.Year.ToString()));
                dt.Rows.Clear();
                dt = MainClass.getBranchList();
                selectFilterTab1Branch.DataSource = dt;
                selectFilterTab1Branch.DataTextField = "INITNAME";
                selectFilterTab1Branch.DataValueField = "ID";
                selectFilterTab1Branch.DataBind();
                selectFilterTab1Branch.SelectedIndex = selectFilterTab1Branch.Items.IndexOf(selectFilterTab1Branch.Items.FindByValue(userData.USR_HELTESID.ToString()));
                dt.Rows.Clear();
                List<string> tempList = new List<string>();
                tempList.Add(userData.USR_HELTESID.ToString());
                dt = MainClass.getStaffsWithBranchPos(tempList, "Сонго...");
                selectFilterTab1Staff.DataSource = dt;
                selectFilterTab1Staff.DataTextField = "ST_NAME";
                selectFilterTab1Staff.DataValueField = "STAFFS_ID";
                selectFilterTab1Staff.DataBind();
                selectFilterTab1Staff.SelectedIndex = selectFilterTab1Staff.Items.IndexOf(selectFilterTab1Staff.Items.FindByValue(userData.USR_STAFFID.ToString()));
                spanReportHeaderDate.InnerHtml = DateTime.Today.ToString("yyyy-MM-dd");
            }
            catch (cs.HRMISException ex)
            {
                myObjGetTableData.exeptionMethod(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                myObjGetTableData.exeptionMethod(ex);
                throw ex;
            }
        }
    }
}