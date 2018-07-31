using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using ServiceInterface.Common;
using Leo;

namespace E_Commerce
{
    /// <summary>
    /// ServiceECommerce 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceECommerce : System.Web.Services.WebService
    {
//        //定义用户身份验证类变量authHeader
//        public AuthHeader authHeader = new AuthHeader();

//        #region 更新物资采购
//        /// <summary>
//        /// 更新物资采购
//        /// </summary>
//        /// <param name="strPurchaseOrderId">采购单ID</param>
//        /// <param name="strGoodsId">货物ID</param>
//        /// <param name="strActualGoodsNum">实际收货数量</param>
//        /// <returns>提示消息</returns>
//        [WebMethod]
//        [SoapHeader("authHeader")]
//        public Package<bool> UpdateMaterialsPurchasing(string strPurchaseOrderId, string strGoodsId, int strActualGoodsNum)
//        {
//            try
//            {
//                //验证是否有权访问
//                if (!authHeader.ValideUser(authHeader.UserName, authHeader.PassWord))
//                {
//                    return new Package<bool>(false, "没有访问权限！", false);
//                }

//                //校验输入参数
//                if (string.IsNullOrWhiteSpace(strPurchaseOrderId) || string.IsNullOrWhiteSpace(strGoodsId) || string.IsNullOrWhiteSpace(Convert.ToString(strActualGoodsNum)))
//                {
//                    return new Package<bool>(true, "采购单ID、货物ID、实际收货数量不能为空！", true);
//                }

//                ////校验采购单货物数据
//                //string strSql =
//                //    string.Format("select * from BO_WZCGMX where BINDID='{0}' and ID='{1}'", strPurchaseOrderId, strGoodsId);
//                //var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteTable(strSql);
//                //if (dt.Rows.Count <= 0)
//                //{
//                //    return new Package<bool>(false, "采购单ID、货物ID不存在！", false);
//                //}

//                ////校验数量
//                //int sumNum = 0;
//                //strSql =
//                //    string.Format("select SL from BO_DDMX where CGDID='{0}' and CPID='{1}'", strPurchaseOrderId, strGoodsId);
//                //dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteTable(strSql);
//                //if (dt.Rows.Count > 0)
//                //{
//                //    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
//                //    { 
//                //        sumNum += Convert.ToInt32(dt.Rows[iRow]["SL"]);
//                //    }                 
//                //}
//                //if (sumNum != strActualGoodsNum)
//                //{
//                //    return new Package<bool>(false, "实际收货数量与订单明细数量不相符！", false);
//                //}
                
//                //strSql =
//                //    string.Format("update BO_WZCGMX set SJDH='{0}', where BINDID='{0}' and ID='{1}'", Convert.ToString(strActualGoodsNum), strPurchaseOrderId, strGoodsId);
//                //new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteNonQuery(strSql);

//                return new Package<bool>(true, null, true);
//            }
//            catch (Exception ex)
//            {
//                return new Package<bool>(false, string.Format("{0}：修改数据发生异常。{1}", ex.Source, ex.Message), false);
//            }
//        }

//        #endregion

//        #region 更新采购订单明细
//        /// <summary>
//        /// 更新采购订单明细
//        /// </summary>
//        /// <param name="strPurchaseOrderId">采购单ID</param>
//        /// <param name="strGoodsId">货物ID</param>
//        /// <param name="strOrderId">订单ID</param>
//        /// <param name="strOrderState">订单状态</param>
//        /// <param name="strNum">数量</param>
//        /// <returns>提示消息</returns>
//        [WebMethod]
//        [SoapHeader("authHeader")]
//        public Package<bool> UpdatePurchasingOrderDetail(string strPurchaseOrderId, string strGoodsId, string strOrderId, string strOrderState, int strNum)
//        {
//            try
//            {
//                //验证是否有权访问
//                if (!authHeader.ValideUser(authHeader.UserName, authHeader.PassWord))
//                {
//                    return new Package<bool>(false, "没有访问权限！", false);
//                }

//                if (string.IsNullOrWhiteSpace(strPurchaseOrderId) || string.IsNullOrWhiteSpace(strGoodsId) || string.IsNullOrWhiteSpace(strOrderId) || string.IsNullOrWhiteSpace(strOrderState) || string.IsNullOrWhiteSpace(Convert.ToString(strNum)))
//                {
//                    return new Package<bool>(false, "采购单ID、货物ID、订单ID、订单状态、订单数量不能为空！", false);
//                }

////                string strSql =
////                    string.Format("select * from BO_DDMX where DDID='{0}'", strOrderId);
////                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteTable(strSql);
////                if (dt.Rows.Count <= 0)
////                {
////                    strSql =
////                      string.Format(@"insert into BO_DDMX(CGDID,CPID,DDID,DDZT,SL) 
////                                      values('{0}','{1}','{2}','{3}','{4}')",
////                                      strPurchaseOrderId, strGoodsId, strOrderId, strOrderState, Convert.ToString(strNum));
////                }
////                else
////                {
////                    strSql =
////                      string.Format(@"update BO_DDMX set DDZT='{0}',SL='{1}' 
////                                      where CGDID='{2}' and CPID='{3}' and DDID='{4}'",
////                                      strOrderState, Convert.ToString(strNum), strPurchaseOrderId, strGoodsId, strOrderId);
////                }
////                new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteNonQuery(strSql);

//                return new Package<bool>(true, null, true);
//            }
//            catch (Exception ex)
//            {
//                return new Package<bool>(false, string.Format("{0}：修改数据发生异常。{1}", ex.Source, ex.Message), false);
//            }
//        }

//        #endregion

        [WebMethod]
        public string GetPurchasingOrderDetail()
        {
            try
            {
                var strSql =
                    string.Format(
                        "select spmc,sl,ddzt,to_char(cjsj, 'yyyy/mm/dd hh24:mi:ss') as cjsj from BO_WZCGMX_DDMX order by cjsj desc");
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathAwstest).ExecuteTable(strSql);
                var xml = new Leo.Xml.ToXml("").GetData(dt);
                
                return new Leo.Xml.Message("获取成功！").TrueXml(xml);
            }
            catch (Exception ex)
            {
                LogTool.WriteLog(typeof(ServiceECommerce), ex);
                return new Leo.Xml.Message(string.Format("获取数据异常。{0}", ex.Message)).FalseXml();
            }
        }
    }
}
