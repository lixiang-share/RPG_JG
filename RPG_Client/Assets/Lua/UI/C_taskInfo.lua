C_taskInfo = {};
local this = C_taskInfo;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst,false)
	inst:S('curPos' , 'Right')
end
function this.OnEnable()
	
end
function  this.OnDisable()
	
end

function this.Show()
	inst:SetX(800)
	inst:S('curPos' , 'Center')
	UITools.SA(inst,true)
	UITools.SA(inst:GetChild('s_mask') , false)
	UITools.TweenPos_X(inst , 0)
end
function this.Cancel()
	inst:SetX(0)
	UITools.SA(inst:GetChild('s_mask') , false)
	UITools.TweenPos_X(inst , 800)
	inst:S('curPos' , 'Right')
end
function this.OnCommand(command , param)
	if command == 'EndTween' then
		if inst:G('curPos') == 'Right' then
			UITools.SA(inst,false)
		else
			UITools.SA(inst:GetChild('s_mask') , true)
		end
	end
end

