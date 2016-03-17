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
	inst:CreateMsg():SetMsgType(MsgProtocol.Get_EquipList):Send()
end
function this.Cancel()
	UITools.ClosePanel(inst)
	UITools.ClosePanel(UITools.D('playerEquips'))
end

function this.OnReceiveData(data)
	local equipList = UITools.MsgToEquipItem(data)
	UITools.Log(equipList.Count)
	this.ParseData(equipList)
end

function this.ParseData(equipList)
	local equips = inst:GetChild('c_equips')
	for i=0,27 do
		local item = equips:GetChild('equip_item'..i)
		if i < equipList.Count then
			UITools.SA(item , true)
			local info = equipList:get_Item(i)
			item:S('info' , info)
			UITools.Set(item:GetChild('s_icon'),info.Icon)
			UITools.Set(item:GetChild('l_amount'),''..info.Amount)
		else
			UITools.SA(item , false)
		end
	end
end

function this.CleanUp()
	-- body
end

function this.Sale(item)
	-- body
end