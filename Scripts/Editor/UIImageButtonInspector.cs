//----------------------------------------------
// 2013-5-25
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UISprites.
/// </summary>

//���������ֶ�����Ա��û�Editor class���༭
[CustomEditor(typeof(UIImageButton))]

//�̳���Editor��EditorGUI��EditorGUILayout�����ڴ����༭���ؼ�
public class UIImageButtonInspector : Editor
{
	UIImageButton mButton;
	UISprite mSprite;

	/// <summary>
	/// Atlas selection callback.
	/// </summary>
	
	//��ѡ���µ�atlas��ͼ��ʱ�Ļص�����
	void OnSelectAtlas (MonoBehaviour obj)
	{
		if (mButton.target != null)
		{
			NGUIEditorTools.RegisterUndo("Atlas Selection", mButton.target);
			mButton.target.atlas = obj as UIAtlas;
			mButton.target.MakePixelPerfect();
		}
	}

	//������Editor�����ڻ����Զ���inspector��ע�⣬�������ֻ���ڳ�����ѡ���˶�����ڼ���������е�����ťʱ�Żᱻִ�У���ִ�д��������������ĴΡ�
	public override void OnInspectorGUI ()
	{
		//EditorGUIUtility���������趨Editor ��Ĭ�������ʽ�������ǰѱ�ǩ(label)�Ŀ����Ϊ80f
		EditorGUIUtility.LookLikeControls(80f);
		//target��Editor�еĳ�Ա������ָ����ǵ�ǰ�����б�ѡ�еĶ���
		mButton = target as UIImageButton;
		//�ڼ�������д���һ�������򣬿�ͨ����ק�ȷ�ʽ���и�ֵ
		mSprite = EditorGUILayout.ObjectField("Sprite", mButton.target, typeof(UISprite), true) as UISprite;
		
		//��Sprite���е�ֵ�뵱ǰbutton�е�target��һֵʱ������
		if (mButton.target != mSprite)
		{
			NGUIEditorTools.RegisterUndo("Image Button Change", mButton);
			mButton.target = mSprite;
			if (mSprite != null) mSprite.spriteName = mButton.normalSprite;
		}

		if (mSprite != null)
		{
			//����EditorGUILayout.ObjectField ��֧���Զ������(components), �˺����Զ����˶����򣬵��ǽ�֧�ִ���ʷ��¼��ѡ��
			ComponentSelector.Draw<UIAtlas>(mSprite.atlas, OnSelectAtlas);
			
			//��������sprite�򣬲�����ص�����
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