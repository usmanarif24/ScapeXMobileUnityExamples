using UnityEngine;
using UnityEngine.UI;

namespace UnityToolkit.SDF
{
    public enum Shape
    {
        Circle = 0,
        Rectangle = 1,
        EquiTriangle = 2
    }
    
    [RequireComponent(typeof(Mask))]
    [ExecuteInEditMode]
    public class PrimitiveSDF : MonoBehaviour
    {

        [SerializeField] private Shape _shape;
        [Range(0.0f, 1.0f)] [SerializeField] private float _fill = 1f;
        [Range(0f, 1f)] [SerializeField] private float _cornerRadius = 1f;
        [Range(0f, 1f)] [SerializeField] private float _smoothness = 0.01f;
        [SerializeField] private bool _isMask = false;

        private Mask _mask;
        private Mask Mask
        {
            get
            {
                if(_mask == null)
                {
                    _mask = GetComponent<Mask>();
                }
                return _mask;
            }

        }

        public bool IsMask
        {
            get => _isMask;
            set
            {
                _isMask = value;
                Mask.enabled = _isMask;
                var material = IsMask ? Image.material : Image.materialForRendering;
                material.SetInt(IsMaskId, _isMask ? 1 : 0);
            }
        }

        public float Fill
        {
            get => _fill;
            set
            {
                _fill = Mathf.Clamp(value, 0f, 1f);
                var material = IsMask ? Image.material : Image.materialForRendering;
                material.SetFloat(FillId, _fill);
            }
        }

        public float CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = Mathf.Clamp(value, 0f, 1f);
                var material = IsMask ? Image.material : Image.materialForRendering;
                material.SetFloat(Radius, _cornerRadius);
            } 
        }

        public float Smoothness
        {
            get => _smoothness;
            set
            {
                _smoothness = Mathf.Clamp(value, 0f, 1f);
                var material = IsMask ? Image.material : Image.materialForRendering;
                material.SetFloat(Smooth, _smoothness);
            }
        }
        
        protected Shader Shader;

        [SerializeField] private bool _debug;
        private static readonly int SizeId = Shader.PropertyToID("_Size");
        private static readonly int FillId = Shader.PropertyToID("_Fill");
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int DebugId = Shader.PropertyToID("_Debug");
        private static readonly int Smooth = Shader.PropertyToID("_Smooth");
        private static readonly int Shape = Shader.PropertyToID("_Shape");
        private static readonly int IsMaskId = Shader.PropertyToID("_IsMask");
        

        protected Vector2 RectSize;
        
        private RawImage _image;
        protected RawImage Image
        {
            get
            {
                if (_image == null)
                {
                    _image = TryGetComponent(out RawImage image) ? image : gameObject.AddComponent<RawImage>();
                    Shader = Shader.Find("SDF/SceneSDF");
                    _image.material = new Material(Shader);
                }

                return _image;
            }
        }
    
        private RectTransform _rectTransform;

        private RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

        private void Awake()
        {
            Mask.showMaskGraphic = false;
        }

        private void Start()
        {
            UpdateSDF();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateSDF();
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateSDF();
        }
#endif

        public void UpdateSDF()
        {
            var size = RectTransform.rect.size;
            var rect = new Rect();
            RectSize = GetSize(size);
            rect.size = RectSize;
            Image.uvRect = rect;
            var s = new Vector4(RectSize.x, RectSize.y, 0f, 0f);
            
            var material = IsMask ? Image.material : Image.materialForRendering;
            
            material.SetInt(Shape, (int)_shape);
            material.SetVector(SizeId, s);
            material.SetFloat(FillId, _fill);
            material.SetInt(DebugId, _debug?1:0);
            material.SetFloat(Radius, _cornerRadius);
            material.SetFloat(Smooth, _smoothness);
            material.SetInt(IsMaskId, _isMask ? 1 : 0);
            Mask.enabled = IsMask;
        }

        private Vector2 GetSize(Vector2 rectSize)
        {
            float aspect;
            if (rectSize.x > rectSize.y)
            {
                aspect = rectSize.x / rectSize.y;
                return new Vector2(aspect, 1.0f);
            }

            aspect = rectSize.y / rectSize.x;
            return new Vector2(1.0f, aspect);
        }
    }
}
