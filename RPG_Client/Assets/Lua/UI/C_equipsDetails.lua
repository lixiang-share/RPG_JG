C_equipsDetails = {};
local this = C_equipsDetails;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst,false)
end
function this.Cancel()
	UITools.SA(UITools.D('playerEquips'):GetChild('s_player') , true)
	UITools.ClosePanel(inst)
end

function this.OnEnable()
	UITools.SA(UITools.D('playerEquips'):GetChild('s_player') , false)
end
function this.Show_Dress(item)
	UITools.SA(inst:GetChild('b_unuse'),false)
	UITools.SA(inst:GetChild('b_upgrade_dressed'),false)
	UITools.SA(inst:GetChild('b_sale'),true)
	UITools.SA(inst:GetChild('b_upgrade'),true)
	UITools.SA(inst:GetChild('b_use'),true)
	inst:S('info' , item)
	UITools.ShowPanel(inst)
	this.ParseData(item)
end

function this.Show_Dressed( item)
	UITools.SA(inst:GetChild('b_unuse'),true)
	UITools.SA(inst:GetChild('b_upgrade_dressed'),true)
	UITools.SA(inst:GetChild('b_sale'),false)
	UITools.SA(inst:GetChild('b_upgrade'),false)
	UITools.SA(inst:GetChild('b_use'),false)
	inst:S('info' , item)
	UITools.ShowPanel(inst)
	this.ParseData(item)
end
function this.ParseData(item)
	inst:GetChild('l_name').Value = item.Name
	inst:GetChild('l_damage').Value = '伤害: '..item.Damage
	inst:GetChild('l_quality').Value = '品质: '..item.Quality
	inst:GetChild('l_hp').Value = '生命值: '..item.Hp
	inst:GetChild('s_icon').Value = item.Icon
	UITools.SA(inst:GetChild('s_icon'):GetChild('s_fore') , item.IsDress)
	inst:GetChild('s_icon'):GetChild('c_stars'):OnCommand('Show' , item.Star)
	inst:GetChild('l_desc').Value = item.Desc
	inst:GetChild('l_level').Value = '等级: '..item.Level
	inst:GetChild('l_price').Value = '售价: '..item.Price
end

function this.Sale()
	if inst:G('info') ~= nil then
		UITools.D('backpack'):CallLuaMethod('Sale' , inst:G('info'))
	else
		UITools.ShowMsg('当前没有相关物品可出售')
	end
	this.Cancel()
end
function this.Use()
	if inst:G('info') ~= nil then
		UITools.D('backpack'):CallLuaMethod('Use' , inst:G('info'))
	else
		UITools.ShowMsg('当前没有相关物品可使用')
	end
	this.Cancel()
end
function this.Upgrade( )
	if inst:G('info') ~= nil then
		UITools.D('backpack'):CallLuaMethod('Upgrade' , inst:G('info'))
	else
		UITools.ShowMsg('当前没有相关物品可升级')
	end
	this.Cancel()
end

function this.Unuse(item)
		if inst:G('info') ~= nil then
		UITools.D('backpack'):CallLuaMethod('Unuse' , inst:G('info'))
	else
		UITools.ShowMsg('当前没有相关物品可使用')
	end
	this.Cancel()
end