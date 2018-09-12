using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Color(uTools)")]
	public class uTweenColor : uTweener {

		public Color from = Color.white;
		public Color to = Color.white;
		public bool includeChilds = false;

		private Text mText;
		private Light mLight;
		private Material mMat;
		private Image mImage;
		private SpriteRenderer mSpriteRender;

		Color mColor = Color.white;

		public Color colorValue {
			get {
				return mColor;
			}
			set {
				SetColor(transform, value);
				mColor = value;
			}
		}

        void Awake() {
            var _transform = this.transform;
            mText = _transform.GetComponent<Text>();
            mLight = _transform.GetComponent<Light>();
            mImage = _transform.GetComponent<Image>();
            mSpriteRender = _transform.GetComponent<SpriteRenderer>();
            var renderer = _transform.GetComponent<Renderer>();
            if (renderer != null)
            {
                mMat = renderer.material;
            }
        }

        protected override void OnUpdate (float factor, bool isFinished)
		{
			colorValue = Color.Lerp(from, to, factor);
		}

		public static uTweenColor Begin(GameObject go, float duration, float delay, Color from, Color to) {
			uTweenColor comp = uTweener.Begin<uTweenColor>(go, duration);
			comp.from = from;
			comp.to = to;
			comp.delay = delay;
			
			if (duration <=0) {
				comp.Sample(1, true);
				comp.enabled = false;
			}
			return comp;
		}

		void SetColor(Transform _transform, Color _color) {
			if (mText != null){
				mText.color = _color;
			}
			if (mLight != null){ 
				mLight.color = _color;
			}
			if (mImage != null) {
				mImage.color = _color;
			}
			if (mSpriteRender != null) {
				mSpriteRender.color = _color;
			}
			if (mMat != null) {
				mMat.color = _color;
			}
			if (includeChilds) {
				for (int i = 0; i < _transform.childCount; ++i) {
					Transform child = _transform.GetChild(i);
					SetColor(child, _color);
				}
			}
			
		}
	}
}
