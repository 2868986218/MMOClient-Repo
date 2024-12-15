namespace SUProtocol {
    public enum CMD {
        None,

        OnClient2LoginConnected,
        OnClient2LoginDisConnected,
        OnClient2BattleConnected,
        OnClient2BattleDisConnected,

        ReqAcctLogin, RspAcctLogin,
        NtfEnterStage, FinEnterStage,
        NtfRoleDatas,

        ReqRoleToken, RspRoleToken,
        ReqTokenAccess, RspTokenAccess,

        SynMovePos,
        PshStateData,

        #region Battle进程内部协议
        B2B_SyncView,
        B2B_SyncCore,
        #endregion
    }

    public enum ErrorCode {
        None,

        //common
        acct_not_exist,

        /// <summary>
        /// 当前区服战斗进程离线
        /// </summary>
        battle_process_disconnected,

        //ReqAcctLogin
        /// <summary>
        /// 账号已在login登录
        /// </summary>
        acct_online_login,
        /// <summary>
        /// 账号已在data登录
        /// </summary>
        acct_online_data,
        /// <summary>
        /// 账号对应data区服不存在
        /// </summary>
        acct_l2d_offline,
        /// <summary>
        /// 账号已封禁
        /// </summary>
        //acct_forbidden,

        //ReqTokenAccess
        token_already_online,
        token_error,
        token_not_exist,
        /// <summary>
        /// token已经过期
        /// </summary>
        token_expired,
    }
}