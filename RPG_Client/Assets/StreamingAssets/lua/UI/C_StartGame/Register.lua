Register = {};
local this = Register;
local inst;


function this.Awake()
	inst = this.inst;
end

function  this.Start()
	UITools.SA(inst,false);
end

function  this.Cancel()
	UITools.ClosePanel(inst);
end

function  this.Register()
	
end

function  this.OnCommand(command , param)
	if command == "Show" then
		UITools.ShowPanel(inst);
	elseif command == "Cancel" then
		this.Cancel();
	elseif command == "Register" then
		this.Register();
	end
end


