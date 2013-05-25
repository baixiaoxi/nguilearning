//----------------------------------------------
// 2013-5-25
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UISprites.
/// </summary>

//定义哪哪种对象可以被用户Editor class所编辑
[CustomEditor(typeof(UIImageButton))]

//继承自Editor，EditorGUI和EditorGUILayout可用于创建编辑器控件
public class UIImageButtonInspector : Editor
{
	UIImageButton mButton;
	UISprite mSprite;

	/// <summary>
	/// Atlas selection callback.
	/// </summary>
	
	//当选择新的atlas贴图集时的回调函数
	void OnSelectAtlas (MonoBehaviour obj)
	{
		if (mButton.target != null)
		{
			NGUIEditorTools.RegisterUndo("Atlas Selection", mButton.target);
			mButton.target.atlas = obj as UIAtlas;
			mButton.target.MakePixelPerfect();
		}
	}

	//函数自Editor，用于绘制自定的inspector。注意，这个函数只有在场景中选择了对象或都在检视面板中有单击或按钮时才会被执行，且执行次数往往会有三四次。
	public override void OnInspectorGUI ()
	{
		//EditorGUIUtility类中用于设定Editor 的默认外观样式，这里是把标签(label)的宽度设为80f
		EditorGUIUtility.LookLikeControls(80f);
		//target是Editor中的成员变量，指向的是当前场景中被选中的对象
		mButton = target as UIImageButton;
		//在检视面板中创建一个对象域，可通过拖拽等方式进行赋值
		mSprite = EditorGUILayout.ObjectField("Sprite", mButton.target, typeof(UISprite), true) as UISprite;
		
		//当Sprite域中的值与当前button中的target不一值时，设置
		if (mButton.target != mSprite)
		{
			NGUIEditorTools.RegisterUndo("Image Button Change", mButton);
			mButton.target = mSprite;
			if (mSprite != null) mSprite.spriteName = mButton.normalSprite;
		}

		if (mSprite != null)
		{
			//由于EditorGUILayout.ObjectField 不支持自定义组件(components), 此函数自定义了对象域，但是仅支持从历史记录中选择
			ComponentSelector.Draw<UIAtlas>(mSprite.atlas, OnSelectAtlas);
			
			//创建三个sprite域，并分配回调函数
			if (mSprite.atlas != null)
			{
				NGUIEditorTools.SpriteField("Normal" , mSprite.atlas, mButton.normalSprite, OnNormal);
				NGUIEditorTools.SpriteField("Hover"  , mSprite.atlas, mButton.hoverSprite, OnHover);
				NGUIEditorTools.SpriteField("Pressed", mSprite.atlas, mButton.pressedSprite, OnPressed);
			}
		}
	}

	void OnNormal (string spriteName)
	{
		NGUIEditorTools.RegisterUndo("Image Button Change", mButton, mButton.gameObject, mSprite);
		mButton.normalSprite = spriteName;
		mSprite.spriteName = spriteName;
		mSprite.MakePixelPerfect();
		if (mButton.collider == null || (mButton.collider is BoxCollider)) NGUITools.AddWidgetCollider(mButton.gameObject)
		//Repaint any inspectors that shows this editor
		Repaint();
	}

	void OnHover (string spriteName)
	{
		NGUIEditorTools.RegisterUndo("Image Button Change", mButton, mButton.gameObject, mSprite);
		mButton.hoverSprite = spriteName;
		Repaint();
	}

	void OnPressed (string spriteName)
	{
		NGUIEditorTools.RegisterUndo("Image Button Change", mButton, mButton.gameObject, mSprite);
		mButton.pressedSprite = spriteName;
		Repaint();
	}
}