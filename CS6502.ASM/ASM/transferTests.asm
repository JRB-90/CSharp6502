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

    LDA #$AA
    TAY
    TAX
    
    LDA #$00
    LDX #$55
    TXA

    LDA #$00
    LDY #$A5
    TYA

    LDX #$00
    TSX
    LDX #$FF
    TXS

    BRK

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
