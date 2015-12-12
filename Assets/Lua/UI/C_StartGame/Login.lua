Login = {};
local this = Login;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.Start()
	
end


function this.OnEnable()
	
end

function this.OnClick()
	
end

function  this.OnCommand(command , param)
	if command == "MoveLeft" then
		UITools.TweenPos_X(inst,-540);
	elseif command == "EndTween" then
		UITools.SA(inst , false);
	end

end

