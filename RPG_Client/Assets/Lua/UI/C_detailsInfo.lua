C_detailsInfo = {};
local this = C_detailsInfo;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	inst:S('curPos' , 'Up')
	UITools.SA(inst , false)
end
function this.OnEnable()
	
end

function  this.OnDisable( )
	
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
			UITools.SA(inst,false)
		else
			UITools.SA(inst:GetChild('s_mask') , true)
		end
	end
end

