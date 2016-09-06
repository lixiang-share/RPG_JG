C_playerEquips = {};
local this = C_playerEquips;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst , false)
end

function this.ClickEquipItem(item)
	UITools.D('equipsDetails'):CallLuaMethod('Show_Dressed' , item)
end

function this.ParseData(equipList)
	local equips = inst:GetChild('c_equips')
	local curIndex = 0
	for i=0,equipList.Count - 1 do
		local info = equipList:get_Item(i)
		if info.IsDress then
			local item = equips:GetChild('equip_item'..curIndex) 
			UITools.SA(item , true)
			item:S('info' , info)
			UITools.Set(item:GetChild('s_icon'),info.Icon)
			UITools.Set(item:GetChild('l_amount'),''..info.Amount)
			curIndex = curIndex + 1
		end
	end
	for i=curIndex,7 do
		local item = equips:GetChild('equip_item'..i)
		UITools.SA(item , false)
	end
	local player = inst.PlayerMgr.Player
	UITools.Log('player.Damage: '..player.Damage)
	inst:GetChild('l_damage').Value = player.Damage
	inst:GetChild('l_hp').Value = player.Hp
	inst:GetChild('l_title').Value = player.Username
	inst:GetChild('experience'):GetChild('l_info').Value = player.Exp ..'/'..player.CurNeedExp
	inst:GetChild('experience'):GetChild('experienceBar').Value = player.Exp/player.CurNeedExp
	local role = inst.PlayerMgr.Role
	UITools.SA(inst:GetChild('s_player'):GetChild('boy_stand'),role.Gender == RoleItem.Man)
	UITools.SA(inst:GetChild('s_player'):GetChild('girl_stand'),role.Gender == RoleItem.Woman)
end
