using System.Collections.Generic;

namespace WeChat.Core.Api.MenuManage.Models
{

    /// <summary>
    /// 菜单模型
    /// </summary>
    public class CreateMenusWrapperModel
    {
        /// <summary>
        /// 一级菜单数组的个数应为1~3个
        /// </summary>
        public List<CreateBaseMenuModel> button { get; set; }
    }

    public abstract class CreateBaseMenuModel
    {
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }
    }

    public abstract class CreateMenuModel : CreateBaseMenuModel
    {
        /// <summary>
        /// 菜单的响应动作类型
        /// </summary>
        public abstract string type { get; }
    }

    /// <summary>
    /// 带有子菜单的一级菜单
    /// </summary>
    public sealed class CreateMenuHasSubModel : CreateBaseMenuModel
    {
        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        public List<CreateMenuModel> sub_button { get; set; }

        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="menuName">菜单名</param>
        /// <param name="subMenus">子菜单</param>
        public CreateMenuHasSubModel(string menuName, List<CreateMenuModel> subMenus)
        {
            this.name = menuName;
            this.sub_button = subMenus;
        }
    }

    /// <summary>
    /// 超链接按钮
    /// </summary>
    public sealed class CreateLinkMenuModel : CreateMenuModel
    {
        public override string type
        {
            get { return "view"; }
        }

        /// <summary>
        /// 网页链接，用户点击菜单可打开链接，不超过256字节
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="menuName">菜单名</param>
        /// <param name="urlAddress">链接地址</param>
        public CreateLinkMenuModel(string menuName, string urlAddress)
        {
            this.name = menuName;
            this.url = urlAddress;
        }
    }

    /// <summary>
    /// 响应类按钮
    /// </summary>
    public sealed class CreateKeyMenuModel : CreateMenuModel
    {
        public override string type
        {
            get { return "click"; }
        }
        /// <summary>
        /// 菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="menuName">菜单名</param>
        /// <param name="keyValue">值</param>
        public CreateKeyMenuModel(string menuName, string keyValue)
        {
            this.name = menuName;
            this.key = keyValue;
        }
    }

    /// <summary>
    /// 扫码带提示按钮
    /// </summary>
    public sealed class ScanQrWaitMenuModel : CreateMenuModel
    {
        public override string type { get { return "scancode_waitmsg"; } }
        public string key { get; set; }

        public ScanQrWaitMenuModel(string menuName, string keyValue)
        {
            this.name = menuName;
            this.key = keyValue;
        }
    }

    /// <summary>
    /// 扫码推事件
    /// </summary>
    public sealed class ScanQrPushMenuModel : CreateMenuModel
    {
        public override string type { get { return "scancode_push "; } }
        public string key { get; set; }

        public ScanQrPushMenuModel(string menuName, string keyValue)
        {
            this.name = menuName;
            this.key = keyValue;
        }
    }

}
