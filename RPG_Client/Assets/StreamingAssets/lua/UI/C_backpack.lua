C_backpack = {};
local this = C_backpack;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst , false)
end

function this.Show()
	UITools.ShowPanel(inst)
	UITools.ShowPanel(UITools.D('playerEquips'))
	this.Refresh()
end
function this.Cancel()
	UITools.ClosePanel(inst)
	UITools.ClosePanel(UITools.D('playerEquips'))
end

function this.OnReceiveData(data)
	local equipList = ''
	if data.MsgType == MsgProtocol.Get_EquipList then
		equipList = UITools.MsgToEquipItem(data)
	else
		equipList = UITools.MsgToEquipItem(data)
		inst.PlayerMgr:UpdatePlayerinfo(data)
		UITools.D('PlayerInfo'):CallLuaMethod('SetInfo')
		UITools.D('PlayerDetails'):CallLuaMethod('SetInfo')
	end
	
	UITools.Log(equipList.Count)
	if equipList.Count > 0 then
		this.ParseData(equipList)
		UITools.D('playerEquips'):CallLuaMethod('ParseData' , equipList)
	else
		UITools.ShowMsg('当前没有装备')
	end
end

function this.ParseData(equipList)
	local equips = inst:GetChild('c_equips')
	local curIndex = 0
	for i=0,equipList.Count - 1 do
		local info = equipList:get_Item(i)
		if not info.IsDress then
			local item = equips:GetChild('equip_item'..curIndex) 
			UITools.SA(item , true)
			item:S('info' , info)
			UITools.Set(item:GetChild('s_icon'),info.Icon)
			UITools.Set(item:GetChild('l_amount'),''..info.Amount)
			curIndex = curIndex + 1
		end
	end
	for i=curIndex,27 do
		local item = equips:GetChild('equip_item'..i)
		UITools.SA(item , false)
	end
end



function this.ClickEquipItem(item)
	if item.Type == item.Equip then
		UITools.D('equipsDetails'):CallLuaMethod('Show_Dress' , item)
	else
		UITools.D('drugResDetails'):CallLuaMethod('Show_Dress' , item)
	end
end

function this.Refresh()
	inst:CreateMsg():SetMsgType(MsgProtocol.Get_EquipList):Send()
end
function this.Sale( item )
	inst:CreateMsg():SetMsgType(MsgProtocol.Sale_Equip):AddInt(item.EquipId):Send()
end
function this.Use( item )
	inst:CreateMsg():SetMsgType(MsgProtocol.Dress_Equip):AddInt(item.EquipId):Send()
end
function this.Upgrade( item)
	inst:CreateMsg():SetMsgType(MsgProtocol.Upgrade_Equip):AddInt(item.EquipId):Send()
end
function this.Unuse(item)
	inst:CreateMsg():SetMsgType(MsgProtocol.Undress_Equip):AddInt(item.EquipId):Send()
end