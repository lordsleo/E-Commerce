//
//文件名：    GetPurchasingOrderDetail.aspx.cs
//功能描述：  获取采购订单明细数据
//创建时间：  2015/12/14
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Leo;
using ServiceInterface.Common;
using Leo.Oracle;

namespace E_Commerce.Service.MaterialsPurchasing
{
    public partial class GetPurchasingOrderDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var bindId = Request.Params["BindId"];
            //bindId = "369949";

            try
            {
                var strSql =
                    string.Format(@"select spmc,sl,ddzt,to_char(cjsj, 'yyyy/mm/dd hh24:mi:ss') as cjsj 
                                    from BO_TX_DDMX 
                                    where cgdid='{0}'
                                    order by cjsj desc",
                                    bindId);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAws).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    var arry = 0;
                    Json = JsonConvert.SerializeObject(arry);
                    return;
                }

                var arrays = new Leo.Data.Table(dt).ToArray();
                Json = Request.QueryString.Get("callback") + "(" + JsonConvert.SerializeObject(arrays) + ")";
            }
            catch (Exception ex)
            {
                LogTool.WriteLog(typeof(GetPurchasingOrderDetail), ex);
            }
        }
        protected string Json;
    }
}