float _PreviousIn;

void MyFunction_float(float In, out float Out) {
    if (_PreviousIn != In) {
        _PreviousIn = In;
        Out = In;
    }

    Out = _PreviousIn;
}