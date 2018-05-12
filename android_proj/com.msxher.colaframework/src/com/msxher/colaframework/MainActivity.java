package com.msxher.colaframework;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.content.DialogInterface;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
//import android.widget.Toast;

public class MainActivity extends UnityPlayerActivity {

	private static String receiveObj = "GameLauncher";
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// Handle action bar item clicks here. The action bar will
		// automatically handle clicks on the Home/Up button, so long
		// as you specify a parent activity in AndroidManifest.xml.
		int id = item.getItemId();
		if (id == R.id.action_settings) {
			return true;
		}
		return super.onOptionsItemSelected(item);
	}

	public int Add(int a, int b) {
		return a + b;
	}
	
	public void ShowConfirmDialog(){
		 Builder builder = new AlertDialog.Builder(this);
		 builder.setTitle("Cola提示");
		 builder.setMessage("确定退出Cola游戏？");
		 
		 //监听里面的按钮事件
		 builder.setPositiveButton("确定", new DialogInterface.OnClickListener() {
			
			@Override
			public void onClick(DialogInterface dialog, int which) {
//				Toast.makeText(getApplicationContext(),"点击了确定",Toast.LENGTH_SHORT).show();
				UnityPlayer.UnitySendMessage(receiveObj, "ApplicationQuit","0");
			}
		});
		 
		 builder.setNegativeButton("取消", new DialogInterface.OnClickListener() {
			
			@Override
			public void onClick(DialogInterface dialog, int which) {
//				Toast.makeText(getApplicationContext(),"点击了取消",Toast.LENGTH_SHORT).show();				
			}
		});
		 builder.setCancelable(true);
		 AlertDialog dialog = builder.create();
		 dialog.show();
	}
   
}
