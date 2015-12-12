Login_btnRegister = {};
local this = Login_btnRegister;



function this.Awake()

	print("Awake");
end

function this.Start()
	print("Start");
end


function this.OnEnable()
	print("OnEnable");
end

function this.OnClick()
	UITools.D("Login"):OnCommand("MoveLeft");
end


