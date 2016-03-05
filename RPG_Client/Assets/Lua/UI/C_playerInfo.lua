C_playerInfo = {};
local this = C_playerInfo;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	this.SetInfo()
end
function this.OnEnable()
	this.SetInfo()
end

function this.SetInfo()
	local player = inst.PlayerMgr.Player
	--baseInfo
	local baseInfo = inst:GetChild('c_baseInfo')
	baseInfo:GetChild('l_level').Value = ''..player.Level
	baseInfo:GetChild('l_name').Value = ''..player.Username
	local vit = baseInfo:GetChild('vitbar')
	vit:GetChild('l_info').Value = player.Vit..'/'..player.TotalVit
	vit.Value = player.Vit / player.TotalVit

	local toughen = baseInfo:GetChild('toughenbar')
	toughen:GetChild('l_info').Value = player.Toughen..'/'..player.TotalToughen
	toughen.Value = player.Toughen / player.TotalToughen

	inst:GetChild('C_Wealth'):GetChild('c_gold'):GetChild('l_amount').Value = player.GoldCount
	inst:GetChild('C_Wealth'):GetChild('c_diamond'):GetChild('l_amount').Value = player.DiamondCount
end


function  this.OnDisable( )
	
end

function this.ShowDetails( )
	UITools.Log('ShowDetails')
	UITools.D('PlayerDetails'):CallLuaMethod('Show')
end

