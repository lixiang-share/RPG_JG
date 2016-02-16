=========================  Domain  ========================

=========================  OnFirstEnable  ========================

=========================  OnEnble  ========================

=========================  OnDisable  ========================

=========================  OnClick  ========================

UITools.D('ServerSelect'):CallLuaMethod('SetCurSelect' , inst:G('item'))

=========================  OnCommand(param,paramEX)  ========================

=========================  OnHold(param)  ========================

=========================  OnParseData(param)  ========================
inst:S('item' , param)
if param.Count > 20 then
	UITools.Set(inst:GetChild('s_bg') , 's_btn_hot02')
else
	UITools.Set(inst:GetChild('s_bg') , 's_btn_smooth02')
end
local desc = param.Id..'区 '..param.Name
========================= End =========================