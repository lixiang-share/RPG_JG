C_EnterGame_C_StartGame = {};
local this = C_EnterGame_C_StartGame;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.Start()
	
end


function  this.OnCommand(command , param)
	if command == "ChangeRole" then
		inst:OnCommand("MoveLeft");
		UITools.D("SelectRole"):OnCommand("MoveCenter");
	elseif command == "MoveLeft" then
		inst:S('pos','Left');
		UITools.TweenPos_X(inst,-780);
	elseif command == "MoveCenter" then
		UITools.SA(inst,true);
		inst:S('pos','Center');
		UITools.TweenPos_X(inst,0);
	elseif command == "EndTween" then
		if inst:G('pos') == 'Left' then
			UITools.SA(inst,false);
		end
	end
end

