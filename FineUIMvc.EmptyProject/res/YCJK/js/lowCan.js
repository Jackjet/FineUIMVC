// JavaScript Document

var mfs = new Array;

function crflow(x1, y1, x2, y2, tw, th, dt, cc) {
    var mff = new flows(x1, y1, x2, y2, tw, th, dt, cc);
    mfs.push(mff);

}

var flows = function (x1, y1, x2, y2, tw, th, dt, cc) { //x1,y1起点坐标，x2,y2终点坐标，tw是箭头的宽度，th是箭头的高度(底边到顶点的距离)，dt是箭头间的间隔距离，cc是箭头的颜色。这些参数中起终点坐标是必须的，其他都是可选的。
    if (tw == undefined) tw = 5; //默认三角形宽度
    if (th == undefined) th = 7; //默认三角形高度
    if (dt == undefined) dt = 5 //默认三角形间隔
    if (cc == undefined) cc = "#349dff"; //默认三角形颜色

    md = 1; //移动距离

    angt = (y2 - y1) / (x2 - x1);
    if (x1 <= x2) ang = 360 * (Math.atan(angt) / (Math.PI * 2));
    else ang = 180 + 360 * (Math.atan(angt) / (Math.PI * 2));
    //计算箭头线要旋转的角度

    w = Math.sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)); //求两点间距离
    tc = Math.ceil(w / (th + dt));
    //计算两点间要生成多少个三角形

    var newcvs = document.createElement('canvas'); //生成一个画布
    newcvs.style.position = "absolute";
    newcvs.style.display = "block";
    newcvs.setAttribute("width", Math.abs(x2 - x1) + tw); //即使水平或垂直也不能没有高度或宽度，需要加个tw
    newcvs.setAttribute("height", Math.abs(y2 - y1) + tw);

    var cxt = newcvs.getContext("2d"); //生成一串箭头的容器

    if ((x1 <= x2) && (y1 <= y2)) { //终点在起点的左下
        newcvs.style.left = x1 + "px";
        newcvs.style.top = y1 + "px";
        cxt.translate(tw, 0);
    } else if ((x1 > x2) && (y1 <= y2)) { //终点在起点的右下
        newcvs.style.left = x2 + "px";
        newcvs.style.top = y1 + "px";
        xdd = x1 - x2 + tw;
        cxt.translate(xdd, tw);
    } else if ((x1 > x2) && (y1 > y2)) {  //终点在起点的右上
        newcvs.style.left = x2 + "px";
        newcvs.style.top = y2 + "px";
        xdd = x1 - x2;
        ydd = y1 - y2 + tw;
        cxt.translate(xdd, ydd);
    } else if ((x1 <= x2) && (y1 > y2)) {  //终点在起点的左上
        newcvs.style.left = x1 + "px";
        newcvs.style.top = y2 + "px";
        ydd = y1 - y2;
        cxt.translate(0, ydd);
    }

    cxt.rotate(ang * (Math.PI / 180));

    document.getElementById("layoutwarp").appendChild(newcvs);

    this.stp = -th; //箭头初始位置

    this.flowani = function () {
        cxt.clearRect(-th, 0, 3000, 3000);
        cxt.fillStyle = cc;
        for (i = 0; i < tc + 30; i++) { //这个20是个容错数量
            cxt.beginPath();
            cxt.moveTo(this.stp + i * (th + dt), 0);
            cxt.lineTo(this.stp + i * (th + dt) + th, tw / 2);
            cxt.lineTo(this.stp + i * (th + dt), tw);
            cxt.closePath();
            cxt.fill();
        }
        this.stp += md;
        if (this.stp >= th + dt) this.stp = 0;
    }
    this.flowShow = function () {
        newcvs.style.display = "block";
    }
    this.flowHide = function () {
        newcvs.style.display = "none";

    }

}


window.onload = function () {
    setInterval(showani, 60);
}

function showani() {

    for (j = 0; j < mfs.length; j++) {
        mfs[j].flowani();
       
    }
}
