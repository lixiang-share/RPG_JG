=========================  Domain  ========================

=========================  OnFirstEnable  ========================

=========================  OnEnble  ========================

=========================  OnDisable  ========================

=========================  OnClick  ========================

=========================  OnCommand(param,paramEX)  ========================

=========================  OnHold(param)  ========================

=========================  OnParseData(param)  ========================
inst:S('Task' , param)
UITools.Set(inst:GetChild('s_icon') ,param.IconName)

if param.GoldCount > 0 then
	UITools.Set(inst:GetChild('s_rewardIcon') ,'gold')
	UITools.Set(inst:GetChild('c_rewards'):GetChild('s_icon') ,'gold')
	UITools.Set(inst:GetChild('c_rewards'):GetChild('l_amount'),''..param.GoldCount)
else
	UITools.Set(inst:GetChild('s_rewardIcon') ,'diamond')
	UITools.Set(inst:GetChild('c_rewards'):GetChild('s_icon') ,'diamond')
	UITools.Set(inst:GetChild('c_rewards'):GetChild('l_amount') ,''..param.DiamondCount)
end
UITools.Set(inst:GetChild('l_title') , param.Name)
local desc = param.Desc .. ' ('..param.CurStage .. '/'..param.TotalStage..')'
UITools.Set(inst:GetChild('l_desc') , desc)
local bg = ''
local text = ''
if param.Status == 0 then
	bg = 'btn_changName01'
	text ='未开始'
elseif param.Status == 1 then
	bg = 'btn_changName01'
	text ='未开始'
elseif param.Status == 2 then
	bg = 'btn_changName01'
	text ='未开始'
elseif param.Status == 3 then
	bg = 'btn_getRewards03'
	text ='未开始'
end
UITools.Set(inst:GetChild('b_button'):GetChild('s_bg') , bg)
UITools.Set(inst:GetChild('b_button'):GetChild('l_desc') , text)

========================= End =========================