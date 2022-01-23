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

    ; INC
    LDA #$00
    STA $05
    INC $05
    INC $05
    INC $05
    INC $05
    INC $05

    LDA #$00
    STA $05
    CLC

    LDA #$FE
    STA $05
    INC $05
    INC $05
    INC $05
    INC $05
    INC $05

    LDA #$00
    STA $05
    CLC

    LDA #$00
    STA $05
    LDX #$03
    INC $02,X
    INC $02,X
    INC $02,X
    INC $02,X
    INC $02,X

    LDX #$00
    LDA #$00
    STA $05
    CLC

    LDA #$00
    STA $05
    LDX #$FF
    INC $05,X
    INC $05,X
    INC $05,X
    INC $05,X
    INC $05,X

    LDX #$00
    LDA #$00
    STA $05
    CLC
    
    LDA #$00
    STA $5005
    INC $5005
    INC $5005
    INC $5005
    INC $5005
    INC $5005

    LDA #$00
    STA $5005
    CLC

    LDA #$00
    STA $5005
    LDX #$03
    INC $5005,X
    INC $5005,X
    INC $5005,X
    INC $5005,X
    INC $5005,X

    LDX #$00
    LDA #$00
    STA $5005
    CLC

    LDA #$00
    STA $5005
    LDX #$FF
    INC $4F05,X
    INC $4F05,X
    INC $4F05,X
    INC $4F05,X
    INC $4F05,X

    LDX #$00
    LDA #$00
    STA $5005
    CLC

    ; INX/Y
    LDX #$00
    INX
    INX
    INX
    INX
    INX

    CLC

    LDX #$FE
    INX
    INX
    INX
    INX
    INX
    INX

    CLC

    LDY #$00
    INY
    INY
    INY
    INY
    INY

    CLC

    LDY #$FE
    INY
    INY
    INY
    INY
    INY
    INY

    CLC

    ; DEC
    LDA #$02
    STA $05
    DEC $05
    DEC $05
    DEC $05
    DEC $05
    DEC $05

    LDA #$00
    STA $05
    CLC

    LDA #$FE
    STA $05
    DEC $05
    DEC $05
    DEC $05
    DEC $05
    DEC $05

    LDA #$00
    STA $05
    CLC

    LDA #$02
    STA $05
    LDX #$03
    DEC $02,X
    DEC $02,X
    DEC $02,X
    DEC $02,X
    DEC $02,X

    LDX #$00
    LDA #$00
    STA $05
    CLC

    LDA #$02
    STA $05
    LDX #$FF
    DEC $05,X
    DEC $05,X
    DEC $05,X
    DEC $05,X
    DEC $05,X

    LDX #$00
    LDA #$00
    STA $05
    CLC
    
    LDA #$02
    STA $5005
    DEC $5005
    DEC $5005
    DEC $5005
    DEC $5005
    DEC $5005

    LDA #$00
    STA $5005
    CLC

    LDA #$02
    STA $5005
    LDX #$03
    DEC $5005,X
    DEC $5005,X
    DEC $5005,X
    DEC $5005,X
    DEC $5005,X

    LDX #$00
    LDA #$00
    STA $5005
    CLC

    LDA #$02
    STA $5005
    LDX #$FF
    DEC $4F05,X
    DEC $4F05,X
    DEC $4F05,X
    DEC $4F05,X
    DEC $4F05,X

    LDX #$00
    LDA #$00
    STA $5005
    CLC

    ; DEX/Y
    LDX #$00
    DEX
    DEX
    DEX
    DEX
    DEX

    CLC

    LDX #$FE
    DEX
    DEX
    DEX
    DEX
    DEX
    DEX

    CLC

    LDY #$00
    DEY
    DEY
    DEY
    DEY
    DEY

    CLC

    LDY #$FE
    DEY
    DEY
    DEY
    DEY
    DEY
    DEY

    CLC

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
