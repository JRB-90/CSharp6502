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

    ; ASL
    LDA #$AA
    STA $05
    STA $5005
    ASL
    LDA #$00
    ASL $05
    LDA #$00
    ASL $5005

    LDA #$AA
    STA $05
    STA $5005
    LDX #$03
    ASL $02,X
    ASL $5002,X

    LDA #$AA
    STA $05
    STA $5005
    LDX #$FF
    ASL $05,X
    ASL $5005,X

    ; LSR
    LDA #$AA
    STA $05
    STA $5005
    LSR
    LDA #$00
    LSR $05
    LDA #$00
    LSR $5005

    LDA #$AA
    STA $05
    STA $5005
    LDX #$03
    LSR $02,X
    LSR $5002,X

    LDA #$AA
    STA $05
    STA $5005
    LDX #$FF
    LSR $05,X
    LSR $5005,X

    ; ROL
    LDA #$AA
    STA $05
    STA $5005
    ROL
    ROL
    ROL
    LDA #$00
    ROL $05
    ROL $05
    ROL $05
    LDA #$00
    ROL $5005
    ROL $5005
    ROL $5005

    LDA #$AA
    STA $05
    STA $5005
    LDX #$03
    ROL $02,X
    ROL $02,X
    ROL $02,X
    ROL $5002,X
    ROL $5002,X
    ROL $5002,X

    LDA #$AA
    STA $05
    STA $5005
    LDX #$FF
    ROL $05,X
    ROL $05,X
    ROL $05,X
    ROL $5005,X
    ROL $5005,X
    ROL $5005,X

    ; ROR
    LDA #$AA
    STA $05
    STA $5005
    ROR
    ROR
    ROR
    LDA #$00
    ROR $05
    ROR $05
    ROR $05
    LDA #$00
    ROR $5005
    ROR $5005
    ROR $5005

    LDA #$AA
    STA $05
    STA $5005
    LDX #$03
    ROR $02,X
    ROR $02,X
    ROR $02,X
    ROR $5002,X
    ROR $5002,X
    ROR $5002,X

    LDA #$AA
    STA $05
    STA $5005
    LDX #$FF
    ROR $05,X
    ROR $05,X
    ROR $05,X
    ROR $5005,X
    ROR $5005,X
    ROR $5005,X

    LDA #$00
    STA $05
    STA $5005
    LDX #$00

    ; BIT
    LDA #%00000000
    STA $05
    STA $5005
    BIT $05
    BIT $5005

    LDA #%10000000
    STA $05
    STA $5005
    BIT $05
    BIT $5005

    LDA #%01000000
    STA $05
    STA $5005
    BIT $05
    BIT $5005

    LDA #%11000000
    STA $05
    STA $5005
    BIT $05
    BIT $5005

    LDA #$AA
    STA $05
    STA $5005
    BIT $05
    BIT $5005

    LDA #$55
    STA $05
    STA $5005
    BIT $05
    BIT $5005

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
