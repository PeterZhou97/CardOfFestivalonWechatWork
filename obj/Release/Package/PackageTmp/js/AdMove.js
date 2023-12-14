//脚本: 图片Move.js
  var PicMove=new Object();
  PicMove.IsInitialized=false;
  PicMove.ScrollX=0;
  PicMove.ScrollY=0;
  PicMove.MoveWidth=0;
  PicMove.MoveHeight=0;
  PicMove.Resize=function(){
  var winsize=GetPageSize();
    PicMove.MoveWidth=winsize[2];
    PicMove.MoveHeight=winsize[3];
    PicMove.Scroll();}
  PicMove.Scroll=function(){
    var winscroll=getPageScroll();
    PicMove.ScrollX=winscroll[0];
    PicMove.ScrollY=winscroll[1];}
  addEvent(window,"resize",PicMove.Resize);
  addEvent(window,"scroll",PicMove.Scroll);
function AdMove(id){
    if(!PicMove.IsInitialized){
        PicMove.Resize();
        PicMove.IsInitialized=true;
    }
    var obj=document.getElementById(id);
    obj.style.position="absolute";
    var W=PicMove.MoveWidth-obj.offsetWidth;
    var H=PicMove.MoveHeight-obj.offsetHeight;
    var x = W*Math.random(),y = H*Math.random();
    var rad=(Math.random()+1)*Math.PI/6;
    var kx=Math.sin(rad),ky=Math.cos(rad);
    var dirx = (Math.random()<0.5?1:-1), diry = (Math.random()<0.5?1:-1);
    var step = 1;
    var interval;
    this.SetLocation=function(vx,vy){x=vx;y=vy;}
    this.SetDirection=function(vx,vy){dirx=vx;diry=vy;}
    obj.CustomMethod=function(){
        obj.style.left = (x + PicMove.ScrollX) + "px";
        obj.style.top = (y + PicMove.ScrollY) + "px";
        rad=(Math.random()+1)*Math.PI/6;
        W=PicMove.MoveWidth-obj.offsetWidth;
        H=PicMove.MoveHeight-obj.offsetHeight;
        x = x + step*kx*dirx;
        if (x < 0){dirx = 1;x = 0;kx=Math.sin(rad);ky=Math.cos(rad);} 
        if (x > W){dirx = -1;x = W;kx=Math.sin(rad);ky=Math.cos(rad);}
        y = y + step*ky*diry;
        if (y < 0){diry = 1;y = 0;kx=Math.sin(rad);ky=Math.cos(rad);} 
        if (y > H){diry = -1;y = H;kx=Math.sin(rad);ky=Math.cos(rad);} 
    }
    this.Run=function(){
        var delay = 20;
        interval=setInterval(obj.CustomMethod,delay);
        obj.οnmοuseοver=function(){clearInterval(interval);}
        obj.οnmοuseοut=function(){interval=setInterval(obj.CustomMethod, delay);}
    }
}