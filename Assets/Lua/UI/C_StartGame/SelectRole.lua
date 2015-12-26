SelectRole = {};
local this = SelectRole;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.Start()
	
end



function  this.OnCommand(command , param)
	if command == "Return" then
		inst:OnCommand("Right");
		UITools.D("EnterGame"):OnCommand("MoveCenter");

	elseif command == "Right" then
		inst:S('pos', 'Right');
		UITools.TweenPos_X(inst,836);

	elseif command == "MoveCenter" then
		UITools.SA(inst,true);
		inst:S('pos','Center');
		UITools.TweenPos_X(inst,0);
	elseif command == "EndTween" then
		if inst:G('pos') == 'Right' then
			UITools.SA(inst,false);
		end
	end
end


