namespace MornLib.Mono {
    public readonly struct MouseClickSet {
        public static MouseClickSet Invalid => new(false,false,false);
        public readonly bool IsRight;
        public readonly bool IsMiddle;
        public readonly bool IsLeft;

        public MouseClickSet(bool isRight,bool isMiddle,bool isLeft) {
            IsRight  = isRight;
            IsMiddle = isMiddle;
            IsLeft   = isLeft;
        }
    }
}
