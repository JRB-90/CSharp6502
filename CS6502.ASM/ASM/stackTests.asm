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

    LDX #$FF
    TXS

    LDA #$AA
    PHA
    PHP
    LDA #$00
    SEC
    PHP

    CLC
    PLP
    PLP
    PLA

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
