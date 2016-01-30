package jg.rpg.net.handler;
import java.io.UnsupportedEncodingException;
import java.nio.ByteBuffer;
import java.util.Date;

import org.apache.log4j.Logger;

import io.netty.buffer.ByteBuf;
import io.netty.buffer.ReadOnlyByteBuf;
import io.netty.buffer.Unpooled;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import io.netty.util.ReferenceCountUtil;

public class DataEnsureHandler extends SimpleChannelInboundHandler<Object> {
	private Logger logger = Logger.getLogger(getClass());

	@Override
	protected void channelRead0(ChannelHandlerContext ctx, Object msg) throws InterruptedException, UnsupportedEncodingException{
		ByteBuf in = (ByteBuf) msg;
		try{
			logger.debug(in.readableBytes());
			while(in.isReadable()){
				System.out.print((char)in.readByte());
			}
		}finally{
			//ReferenceCountUtil.release(msg);
		}
		logger.debug("start send");
		/*ByteBuf bb= ctx.alloc().buffer("hello client".getBytes().length);
		bb.setBytes(0, "hello client".getBytes());
		//bb.
		//ctx.writeAndFlush(new Date);
		ctx.channel().writeAndFlush("hello client");*/
		
		
		String str = "您已经开启与服务端链接"+" "+new Date()+" "+ctx.channel().localAddress();
		ByteBuf buf = Unpooled.buffer(str.getBytes().length);
		buf.writeBytes(str.getBytes("UTF-8"));
		ctx.writeAndFlush(buf);
		
		logger.debug("end send");
		//Thread.sleep(2000);
     //   ctx.flush();

	}

}
