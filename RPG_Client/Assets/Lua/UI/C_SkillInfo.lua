C_SkillInfo = {};
local this = C_SkillInfo;
local inst;


function this.Awake()
	inst = this.inst;
end
function this.OnFirstEnable()
	UITools.SA(inst , false)
end

function this.OnEnable()
	inst:CreateMsg():SetMsgType(MsgProtocol.Get_SkillList):Send()
end
function this.Show()
	UITools.ShowPanel(inst)
end
function this.Cancel()
	UITools.ClosePanel(inst)
end

function this.Select(item)
	UITools.Log('Select:'..item:ToString())
	local desc = item.Name..' Lv.'..item.Level
	inst:GetChild('l_name').Value = desc
	inst:GetChild('l_desc').Value = item.Desc..'(战斗力: '..item.Fc..')'
	local curSelect = inst:GetChild('b_skill_'..item.Pos)
	curSelect:S('item' , item)
	UITools.DimSprite(curSelect , true)


	if inst:G('curItem') ~= nil and  inst:G('curItem').Pos ~= item.Pos then
		UITools.DimSprite(inst:GetChild('b_skill_'..inst:G('curItem').Pos) , false)
	end
	inst:S('curItem' , item)
end
function this.Upgrade()
	inst:CreateMsg():SetMsgType(MsgProtocol.Upgrade_Skill):AddInt(inst:G('curItem').SkillID):Send()
end

function this.OnReceiveData(data)
	local skills = UITools.MsgToSkillList(data)
	UITools.Log('skill Count : '..skills.Count)
	for i=0,skills.Count - 1 do
		local info = skills:get_Item(i)
		if info.Type ~= 'Base' then
			local  item = inst:GetChild('b_skill_'..info.Pos)
			item.Value = info.Icon
			item:S('item' , info)
			if inst:G('curItem') == nil and info.Pos == 'two' then
				this.Select(info)
			elseif inst:G('curItem') ~= nil and info.Pos == inst:G('curItem').Pos then
				this.Select(info)
			end
		end
	end
	if data.MsgType == MsgProtocol.Upgrade_Skill then
		inst.PlayerMgr:UpdatePlayerinfo()
	end
end
