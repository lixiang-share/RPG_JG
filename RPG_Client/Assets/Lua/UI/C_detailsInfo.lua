C_detailsInfo = {};
local this = C_detailsInfo;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	inst:S('curPos' , 'Up')
	--UITools.SA(inst , false)
	UITools.SA(inst:GetChild('s_mask') , false)
	this.SetInfo()
	this.Recovery()
end


function this.Recovery()
	inst:S('need_update?' , false)
	inst:S('CVitTime' , 0)
	local player = inst.PlayerMgr.Player
	local total = (player.TotalVit - player.Vit) * 60
	inst:S('CVitTotalTime' , total)


	inst:S('CToughenTime' , 0)
	local player = inst.PlayerMgr.Player
	total = (player.TotalToughen - player.Toughen) * 60
	inst:S('CToughenTotalTime' , total)
	inst:DoDelay('StartRecovery' , 1)
end
function this.StartRecovery(time)
	UITools.Log('StartRecovery')
	local player = inst.PlayerMgr.Player
	if player.Vit >= player.TotalVit and player.Toughen >= player.TotalToughen  then
	 	return 
	else
		this.StartRecoveryVit()
		this.StartRecoveryToughen()
		this.SetInfo()
		if inst:G('need_update?') then
			this.UpdatePlayerInfo();
			UITools.D('PlayerInfo'):CallLuaMethod('SetInfo')
			inst:S('need_update?' , false) 
		end
		inst:DoDelay('StartRecovery' , 1)
	end
end

function this.UpdatePlayerInfo()
	local player = inst.PlayerMgr.Player
	inst:CreateMsg():SetMsgType(MsgProtocol.Update_PlayerInfo):AddInt(2):AddString('vit'):AddString(''..player.Vit):AddString('toughen'):AddString(''..player.Toughen):Send()
end

function this.StartRecoveryVit()
	local player = inst.PlayerMgr.Player
	if player.Vit >= player.TotalVit then return end
	inst:S('CVitTotalTime' , inst:G('CVitTotalTime') - 1)
	local CVitTime = inst:G('CVitTime')
	local vit = math.floor(CVitTime / 60)
	if vit > 0 then 
		player.Vit = player.Vit + vit 
		inst:S('CVitTime' , 1) 
		inst:S('need_update?' , true) 
	else 
		inst:S('CVitTime' , inst:G('CVitTime') + 1) 
	end
	local total = this.SecondToTimeString(inst:G('CVitTotalTime'))
	inst:GetChild('c_vit'):GetChild('l_timeTotal').Value = total

end


function this.StartRecoveryToughen()
	local player = inst.PlayerMgr.Player
	if player.Toughen >= player.TotalToughen then return end
	inst:S('CToughenTotalTime' , inst:G('CToughenTotalTime') - 1)
	local CToughenTime = inst:G('CToughenTime')
	local toughen = math.floor(CToughenTime / 60)
	if toughen > 0 then 
		player.Toughen = player.Toughen + toughen  
		inst:S('CToughenTime' , 1)
		inst:S('need_update?' , true) 
	else 
		inst:S('CToughenTime' , inst:G('CToughenTime') + 1) 
	end	
	local total = this.SecondToTimeString(inst:G('CToughenTotalTime'))
	inst:GetChild('c_toughen'):GetChild('l_timeTotal').Value = total
end



function this.SecondToTimeString(seconds)
	local desc = ''
	local h = math.floor(seconds / 3600)
	if h < 10 then desc = desc .. '0'..h..':' else desc = desc .. h .. ':' end
	local m = math.floor((seconds - h* 3600) / 60)
	if m < 10 then desc = desc .. '0'..m..':' else desc = desc .. m ..':' end
	local s = math.floor(seconds - h*3600 - m*60)
	if s < 10 then desc = desc .. '0' .. s else desc = desc .. s end
	return desc
end

function this.OnEnable()
	this.SetInfo()
end

function  this.SetInfo()
	local player = inst.PlayerMgr.Player
	inst:GetChild('l_name').Value = player.Username
	inst:GetChild('l_level').Value = player.Level
	inst:GetChild('vip'):GetChild('l_num').Value = player.Vip
	inst:GetChild('battle'):GetChild('l_num').Value = player.Damage
	inst:GetChild('experience'):GetChild('l_info').Value = player.Exp .. '/'..player.CurNeedExp
	inst:GetChild('experience'):GetChild('experienceBar').Value = player.Exp / player.CurNeedExp
	inst:GetChild('c_gold'):GetChild('l_amount').Value = player.GoldCount
	inst:GetChild('c_diamond'):GetChild('l_amount').Value = player.DiamondCount
	local vit = inst:GetChild('c_vit')
	vit:GetChild('l_physicalRatio').Value = player.Vit .. '/'..player.TotalVit
	
	local toughen = inst:GetChild('c_toughen')
	toughen:GetChild('l_experienceRatio').Value = player.Toughen .. '/'..player.TotalToughen
end
function this.Show()
	inst:S('curPos' , 'Down')
	UITools.SA(inst , true)
	UITools.TweenPos_Y(inst , 50)
end

function this.Close()
	inst:S('curPos' , 'Up')
	UITools.SA(inst:GetChild('s_mask') , false)
	UITools.TweenPos_Y(inst , 500)
end

function this.OnCommand(command , param)
	if command == 'EndTween' then
		if inst:G('curPos') == 'Up' then
			--UITools.SA(inst,false)
			UITools.SA(inst:GetChild('s_mask') , false)
		else
			UITools.SA(inst:GetChild('s_mask') , true)
		end
	end
end

