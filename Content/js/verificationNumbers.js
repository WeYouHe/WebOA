
function showCheck(a) {
	var c = document.getElementById("myCanvas");
  var ctx = c.getContext("2d");
	ctx.clearRect(0,0,1000,1000);
	ctx.font = "80px 'Microsoft Yahei'";
	ctx.fillText(a,0,100);
	ctx.fillStyle = "rgba(255,255,255,.9)";
}
var code ;    
function createCode(){       
    code = "";      
    var codeLength = 4;
    var selectChar = new Array(1,2,3,4,5,6,7,8,9,'a','b','c','d','e','f','g','h','j','k','l','m','n','p','q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','J','K','L','M','N','P','Q','R','S','T','U','V','W','X','Y','Z');      
    for(var i=0;i<codeLength;i++) {
       var charIndex = Math.floor(Math.random()*60);
      code +=selectChar[charIndex];
    }      
    if(code.length != codeLength){
      createCode();      
    }
    showCheck(code);
}

//验证验证码是否一致
function validate() {
    function validate() {
        var inputCode = document.getElementById("J_codetext").value.toUpperCase();
        var codeToUp = code.toUpperCase();
        if (inputCode.length <= 0) {
            document.getElementById("J_codetext").setAttribute("placeholder", "输入验证码");
            createCode();
            return false;
        }
        else if (inputCode != codeToUp) {
            document.getElementById("J_codetext").value = "";
            document.getElementById("J_codetext").setAttribute("placeholder", "验证码错误");
            createCode();
            return false;
        }
        else {
            document.getElementById("J_codetext").value = "";
            createCode();
            return true;
        }

    }
}
//创建2d渲染区域以及初始化
var c = document.getElementById('myCanvas');
var ctx = c.getContext("2d");
var res;
function ran(a, b) {
    return Math.floor(Math.random() * (b - a + 1)) + a;
}
//draw函数
function draw() {
    res = "";
    ctx.clearRect(0, 0, 300, 150);
    //线
    for (var i = 0; i < 80; i++) {
        ctx.strokeStyle = "rgba(" + ran(0, 255) + "," + ran(0, 255) + "," + ran(0, 255) + "," + ran(0, 100) / 100 + ")";
        ctx.beginPath();
        ctx.lineWidth = ran(1, 4);
        ctx.moveTo(ran(0, 300), ran(0, 150));
        ctx.lineTo(ran(0, 300), ran(0, 150));
        ctx.stroke();
    }
    //圆点
    for (var i = 0; i < 80; i++) {
        ctx.beginPath();
        ctx.fillStyle = "rgba(" + ran(0, 255) + "," + ran(0, 255) + "," + ran(0, 255) + "," + ran(0, 100) / 100 + ")";
        ctx.arc(ran(0, 300), ran(0, 150), ran(1, 8), 0, 2 * Math.PI);
        ctx.fill();
    }
    //字符
    var str = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'
    for (var i = 0; i < 4; i++) {
        var strRan = str[ran(0, 35)];
        ctx.fillStyle = "rgb(" + ran(0, 255) + "," + ran(0, 255) + "," + ran(0, 255) + ")"
        ctx.font = ran(50, 80) + "px 微软雅黑"
        ctx.fillText(strRan, i * 50 + 50, 100);
        res += strRan;
    }
    console.log(draw())
}

