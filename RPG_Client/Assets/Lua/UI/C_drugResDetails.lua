C_drugResDetails = {};
local this = C_drugResDetails;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst , false)
end

function this.Cancel()
	UITools.ClosePanel(inst)
end

function this.Show(item)
	inst:S('info' , item)
	UITools.ShowPanel(inst)
	this.Parse(item)
end
function this.Parse(item)
	inst:GetChild('l_title').Value = item.Name
	if item.Type == 'Drug' then
		inst:GetChild('l_type').Value = '药品'
	else
		inst:GetChild('l_type').Value = '宝箱'
	end
	inst:GetChild('s_icon').Value = item.Icon
	inst:GetChild('l_desc').Value = item.Desc
end

function this.Sale()
	
end
function this.Use()
	
end