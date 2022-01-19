.feature force_range
.debuginfo +
.setcpu "65c02"
.macpack longbranch
.list on

.feature org_per_seg
.segment "ZEROPAGE"
.zeropage
.org $0000

; Main code
.segment "CODE"
.org $8000

Reset:
    
    CLC
    CLD
    CLI
    CLV

    JSR Sub1
    JSR Sub2
    JSR Sub3

    BRK

Sub1:
    LDA #$51
    RTS

Sub2:
    LDA #$52
    SED
    RTS

Sub3:
    LDA #$53
    JSR Sub4
    JSR Sub5
    JSR Sub4
    RTS

Sub4:
    LDA #$54
    JSR Sub5
    RTS

Sub5:
    LDA #$55
    RTS

IRQ:
    RTI

NMI:
    RTI

; Interupt vectors
.segment "VECTORS"
.org $FFFA
	.word	NMI
	.word	Reset
	.word	IRQ
