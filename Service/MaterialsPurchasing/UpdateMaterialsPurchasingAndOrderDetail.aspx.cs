//
//文件名：    UpdateMaterialsPurchasingAndOrderDetail.aspx.cs
//功能描述：  更新物资采购和订单明细
//创建时间：  2015/12/04
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
using System.Net;
using System.Text;
using System.Collections;

namespace E_Commerce.Service.MaterialsPurchasing
{
    public partial class UpdateMaterialsPurchasingAndOrderDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //采购单ID
            var strPurchaseOrderId = Request.Params["PurchaseOrderId"];
            //货物ID
            var strGoodsId = Request.Params["GoodsId"];
            //订单ID
            var strOrderId = Request.Params["OrderId"];
            //订单状态
            var strOrderState = Request.Params["OrderState"];
            //数量
            var strNum = Request.Params["Num"];
            //货物名称
            var strGoods = Request.Params["Goods"];
            //创建者账户
            var strCreateUser = Request.Params["CreateUser"];

            //strOrderId = Convert.ToString(contanct.OrderId++);
            //strOrderId = "2015122414219391";
            //strCreateUser = "zhengshuai";
            //strPurchaseOrderId = "369949";
            //strGoodsId = "309841";
            //strGoods = "baijiu";
            //strOrderState = "chenggong";
            //strNum = "12";

            try
            {
                if (string.IsNullOrWhiteSpace(strPurchaseOrderId) || string.IsNullOrWhiteSpace(strGoodsId) || string.IsNullOrWhiteSpace(strOrderId) || string.IsNullOrWhiteSpace(strOrderState) || string.IsNullOrWhiteSpace(Convert.ToString(strNum)) || string.IsNullOrWhiteSpace(strGoods))
                {
                    string warning = string.Format("参数PurchaseOrderId，GoodsId，OrderId，OrderState，Num，Goods不能为空！举例：http://218.92.115.55/E-Commerce/Service/MaterialsPurchasing/UpdateMaterialsPurchasingAndOrderDetail.aspx?PurchaseOrderId=195645&GoodsId=175669&OrderId=123&OrderState=正在发货&Num=10&Goods=电脑");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                // 更新采购订单明细
                Json = UpdatePurchasingOrderDetail(strPurchaseOrderId, strGoodsId, strOrderId, strCreateUser, strOrderState, Convert.ToInt32(strNum), strGoods);
                if (Json != string.Empty)
                {
                    return;
                }
                // 更新物资采购
                Json = UpdateMaterialsPurchasing(strPurchaseOrderId, strGoodsId);
                if (Json != string.Empty)
                {
                    return;
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, null, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：修改数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;

        #region 更新物资采购
        /// <summary>
        /// 更新物资采购
        /// </summary>
        /// <param name="strPurchaseOrderId">采购单ID</param>
        /// <param name="strGoodsId">货物ID</param>
        /// <returns>状态消息</returns>
        private string UpdateMaterialsPurchasing(string strPurchaseOrderId, string strGoodsId)
        {
            string strJson = string.Empty;
            try
            {
                //订单收货数量总和
                int sumOrderGoodsNum = 0;
                //实际收货数量
                int actualGoodsNum = 0;
                //校验采购单货物数据
                string strSql =
                    string.Format("select * from BO_TX_WZCGMX where BINDID='{0}' and ID='{1}'", strPurchaseOrderId, strGoodsId);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAws).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "采购单ID、货物ID不存在！").DicInfo());
                    return strJson;
                }
                actualGoodsNum = string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["SJDH"])) == true ? 0 : Convert.ToInt32(dt.Rows[0]["SJDH"]);

                //校验数量
                strSql =
                    string.Format("select SL from BO_TX_DDMX where CGDID='{0}' and CPID='{1}'", strPurchaseOrderId, strGoodsId);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAws).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        sumOrderGoodsNum += Convert.ToInt32(dt.Rows[iRow]["SL"]);
                    }
                }
                if (sumOrderGoodsNum != actualGoodsNum)
                {
                    strSql =
                        string.Format("update BO_TX_WZCGMX set SJDH='{0}' where BINDID='{1}' and ID='{2}'", sumOrderGoodsNum, strPurchaseOrderId, strGoodsId);
                    new Leo.Oracle.DataAccess(RegistryKey.KeyPathAws).ExecuteNonQuery(strSql);
                }
            }
            catch (Exception ex)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：更新物资采购数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
            return strJson;
        }
        #endregion

        #region 更新采购订单明细
        /// <summary>
        /// 更新采购订单明细
        /// </summary>
        /// <param name="strOrderId">采购单ID</param>
        /// <param name="strOrderId">货物ID</param>
        /// <param name="strOrderId">订单ID</param>
        /// <param name="strOrderId">创建者</param>
        /// <param name="strOrderState">订单状态</param>
        /// <param name="strNum">数量</param>
        /// <param name="strGoods">货物名称</param>
        /// <returns>状态消息</returns>
        private string UpdatePurchasingOrderDetail(string strPurchaseOrderId, string strGoodsId, string strOrderId, string strCreateUser, string strOrderState, int strNum, string strGoods)
        {
            string strJson = string.Empty;
            try
            {
                //校验采购单货物数据
                string strSql =
                    string.Format("select * from BO_TX_WZCGMX where BINDID='{0}' and ID='{1}'", strPurchaseOrderId, strGoodsId);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAws).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "采购单ID、货物ID不存在！").DicInfo());
                    return strJson;
                }

                strSql =
                    string.Format(@"select * from BO_TX_DDMX 
                                    where CGDID='{0}' and CPID='{1}' and DDID='{2}'",
                                    strPurchaseOrderId, strGoodsId, strOrderId);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAws).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    strSql =
                      string.Format(@"insert into BO_TX_DDMX(CGDID,CPID,DDID,DDZT,SL,SPMC) 
                                      values('{0}','{1}','{2}','{3}','{4}','{5}')",
                                      strPurchaseOrderId, strGoodsId, strOrderId, strOrderState, Convert.ToString(strNum), strGoods);
                }
                else
                {
                    strSql =
                      string.Format(@"update BO_TX_DDMX set DDZT='{0}',SL='{1}',SPMC='{2}'
                                      where CGDID='{3}' and CPID='{4}' and DDID='{5}'",
                                      strOrderState, Convert.ToString(strNum), strGoods, strPurchaseOrderId, strGoodsId, strOrderId);

//                    strSql =
//                      string.Format(@"update BO_DDMX set DDZT='{0}',SL='{1}',UPDATEDATE=to_date('{2}','yyyy/mm/dd hh24:mi:ss') 
//                                                      where CGDID='{3}' and CPID='{4}' and DDID='{5}'",
//                                      strOrderState, Convert.ToString(strNum), DateTime.Now, strPurchaseOrderId, strGoodsId, strOrderId);
                }
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathAws).ExecuteNonQuery(strSql);
            }
            catch (Exception ex)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：更新采购订单明细数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
            return strJson;
        }
        #endregion
    }
}


//            //校验采购单货物数据
//            string strSql =
//                string.Format("select * from BO_WZCGMX where BINDID='{0}' and ID='{1}'", strPurchaseOrderId, strGoodsId);
//            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteTable(strSql);
//            if (dt.Rows.Count <= 0)
//            {
//                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "采购单ID、货物ID不存在！").DicInfo());
//                return strJson;
//            }

//            string strOrgNo = Convert.ToString(dt.Rows[0]["ORGNO"]);
//            Int64 strBindId = Convert.ToInt64(dt.Rows[0]["BINDID"]);
//            DateTime curTime = DateTime.Now;
//            string strCreateUser = Convert.ToString(dt.Rows[0]["CREATEUSER"]);
//            string strUpdateUser = Convert.ToString(dt.Rows[0]["UPDATEUSER"]);
//            Int64 strWorkflowId = Convert.ToInt64(dt.Rows[0]["WORKFLOWID"]);
//            Int64 strWorkflowStepId = Convert.ToInt64(dt.Rows[0]["WORKFLOWSTEPID"]);
//            int isEnd = Convert.ToInt32(dt.Rows[0]["ISEND"]);
            
//            strSql =
//                string.Format("select * from BO_DDMX where DDID='{0}'", strOrderId);
//            dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteTable(strSql);
//            if (dt.Rows.Count <= 0)
//            {
//                strSql =
//                  string.Format(@"insert into BO_DDMX(ORGNO,BINDID,CREATEDATE,CREATEUSER,UPDATEDATE,UPDATEUSER,WORKFLOWID,WORKFLOWSTEPID,ISEND,CGDID,CPID,DDID,DDZT,SL) 
//                                  values('{0}',{1},to_date('{2}','yyyy/mm/dd hh24:mi:ss'),'{3}',to_date('{4}','yyyy/mm/dd hh24:mi:ss'),'{5}',{6},{7},{8},'{9}','{10}','{11}','{12}','{13}')",
//                                  strOrgNo,strBindId,curTime,strCreateUser,curTime,strUpdateUser,strWorkflowId,strWorkflowStepId,isEnd,strPurchaseOrderId, strGoodsId, strOrderId, strOrderState, Convert.ToString(strNum));
//            }
//            else
//            {
//                strSql =
//                  string.Format(@"update BO_DDMX set DDZT='{0}',SL='{1}',UPDATEDATE=to_date('{2}','yyyy/mm/dd hh24:mi:ss') 
//                                  where CGDID='{3}' and CPID='{4}' and DDID='{5}'",
//                                  strOrderState, Convert.ToString(strNum), curTime, strPurchaseOrderId, strGoodsId, strOrderId);
//            }





//                string strSql =
//                    string.Format(@"select * from BO_WZCGMX_DDMX 
//                                    where CGDID='{0}' and CPID='{1}' and DDID='{2}'",
//                                    strPurchaseOrderId, strGoodsId, strOrderId);
//                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteTable(strSql);
//                if (dt.Rows.Count <= 0)
//                {
//                    strSql =
//                      string.Format(@"insert into BO_WZCGMX_DDMX(CGDID,CPID,DDID,DDZT,SL,SPMC) 
//                                      values('{0}','{1}','{2}','{3}','{4}','{5}')",
//                                      strPurchaseOrderId, strGoodsId, strOrderId, strOrderState, Convert.ToString(strNum), strGoods);
//                }
//                else
//                {
//                    strSql =
//                      string.Format(@"update BO_WZCGMX_DDMX set DDZT='{0}',SL='{1}',SPMC='{2}'
//                                      where CGDID='{3}' and CPID='{4}' and DDID='{5}'",
//                                      strOrderState, Convert.ToString(strNum), strGoods, strPurchaseOrderId, strGoodsId, strOrderId);

//                    strSql =
//                      string.Format(@"update BO_DDMX set DDZT='{0}',SL='{1}',UPDATEDATE=to_date('{2}','yyyy/mm/dd hh24:mi:ss') 
//                                                      where CGDID='{3}' and CPID='{4}' and DDID='{5}'",
//                                      strOrderState, Convert.ToString(strNum), DateTime.Now, strPurchaseOrderId, strGoodsId, strOrderId);
//                }
//                new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteNonQuery(strSql);







//   string strJson = string.Empty;
//            try
//            {         
//                string strSql =
//                    string.Format(@"select * from BO_DDMX 
//                                    where CGDID='{0}' and CPID='{1}' and DDID='{2}'",
//                                    strPurchaseOrderId, strGoodsId, strOrderId);
//                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteTable(strSql);
//                if (dt.Rows.Count <= 0)//插入
//                {
//                    string content = "{'CGDID':" + strPurchaseOrderId + "',";
//                    content = content + "'CPID':" + "'" + strGoodsId + "',";
//                    content = content + "'DDID':" + "'" + strOrderId + "',";
//                    content = content + "'SL':" + "'" + strNum + "',";
//                    content = content + "'DDZT':" + "'" + strOrderState + "'" + "}";
//                    //AWS
//                    string strURL = "http://168.100.1.244:8099/services/rs/bo/fec70716ce94ae7544783b96eb7c91c5/createBOData";
//                    //string strURL = "http://168.100.1.238/services/rs/bo/ffc42b99be68e6050c3c475aa2339c13/createBOData";
//                    System.Net.HttpWebRequest request;
//                    request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
//                    //Post请求方式
//                    request.Method = "POST";
//                    // 内容类型
//                    request.ContentType = "application/x-www-form-urlencoded";
//                    byte[] payload;
//                    StringBuilder sb = new StringBuilder();
//                    sb.Append("&boTableName=BO_DDMX");
//                    sb.Append("&recordData=" + System.Web.HttpUtility.UrlEncode(content) + "");
//                    sb.Append("&processInstanceId=" + strPurchaseOrderId + "");
//                    sb.Append("&createUser=" + strCreateUser + "");
//                    //将URL编码后的字符串转化为字节
//                    payload = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
//                    //payload = System.Text.Encoding.UTF8.GetBytes(System.Web.HttpUtility.UrlEncode(sb.ToString()));
//                    //payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
//                    //设置请求的 ContentLength 
//                    request.ContentLength = payload.Length;
//                    //获得请 求流
//                    System.IO.Stream writer = request.GetRequestStream();
//                    //将请求参数写入流
//                    writer.Write(payload, 0, payload.Length);
//                    // 关闭请求流
//                    writer.Close();
//                    System.Net.HttpWebResponse response;
//                    // 获得响应流
//                    response = (System.Net.HttpWebResponse)request.GetResponse();
//                    System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
//                    string responseText = myreader.ReadToEnd();
//                    myreader.Close();
//                    if (responseText.Length < 10)
//                    {
//                        strJson = JsonConvert.SerializeObject(new DicPackage(true, null, "更新成功！").DicInfo());
//                    }
//                    else
//                    {
//                        strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "更新失败，OA系统错误信息反馈如下：" + responseText).DicInfo());
//                    }        
//                }
//                else//更新
//                {
//                    strSql =
//                      string.Format(@"update BO_DDMX set DDZT='{0}',SL='{1}',UPDATEDATE=to_date('{2}','yyyy/mm/dd hh24:mi:ss') 
//                                                      where CGDID='{3}' and CPID='{4}' and DDID='{5}'",
//                                      strOrderState, Convert.ToString(strNum), DateTime.Now, strPurchaseOrderId, strGoodsId, strOrderId);
//                }
//                new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteNonQuery(strSql);