ServerSelect = {};
local this = ServerSelect;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.Start()
	UITools.SA(inst,false);
end
function  this.OnCommand(command , param)
	if command == 'Show' then
		UITools.ShowPanel(inst);
	elseif command == 'SelectServer' then
		UITools.Log('SelectServer');
	end
end

